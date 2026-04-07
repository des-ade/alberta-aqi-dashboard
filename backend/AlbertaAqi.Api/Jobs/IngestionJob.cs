using Microsoft.EntityFrameworkCore;
using AlbertaAqi.Api.Data;
using AlbertaAqi.Api.Models;
using AlbertaAqi.Api.Services;

namespace AlbertaAqi.Api.Jobs;

public class IngestionJob
{
    private readonly AqiDbContext _db;
    private readonly OpenAqService _openAq;
    private readonly ILogger<IngestionJob> _logger;

    // Parameters we care about from the schema doc
    private static readonly HashSet<string> TrackedParameters = new()
    {
        "pm25", "pm10", "o3", "no2", "so2", "co",
        "pm1", "temperature", "relativehumidity", "um003"
    };

    public IngestionJob(AqiDbContext db, OpenAqService openAq, ILogger<IngestionJob> logger)
    {
        _db = db;
        _openAq = openAq;
        _logger = logger;
    }

    // Step 1: Sync all Alberta stations and their sensors
    public async Task SyncStationsAsync()
    {
        _logger.LogInformation("Starting station sync...");
        var locations = await _openAq.GetAlbertaStationsAsync();
        var now = DateTime.UtcNow;

        foreach (var location in locations)
        {
            if (location.Coordinates == null) continue;

            // Upsert station — update if exists, insert if new
            var station = await _db.Stations.FindAsync(location.Id);
            if (station == null)
            {
                station = new Station { Id = location.Id };
                _db.Stations.Add(station);
            }

            station.Name = location.Name;
            station.Locality = location.Locality;
            station.Timezone = location.Timezone;
            station.Latitude = location.Coordinates.Latitude;
            station.Longitude = location.Coordinates.Longitude;
            station.IsMonitor = location.IsMonitor;
            station.Provider = location.Provider?.Name;
            station.DatetimeFirst = location.DatetimeFirst?.Utc;
            station.DatetimeLast = location.DatetimeLast?.Utc;

            // Active = reported within last 48 hours
            station.IsActive = station.DatetimeLast.HasValue &&
                (now - station.DatetimeLast.Value).TotalHours <= 48;

            // Sync sensors for this station
            var sensors = await _openAq.GetSensorsForStationAsync(location.Id);
            foreach (var s in sensors)
            {
                if (s.Parameter == null) continue;
                if (!TrackedParameters.Contains(s.Parameter.Name)) continue;

                var sensor = await _db.Sensors.FindAsync(s.Id);
                if (sensor == null)
                {
                    _db.Sensors.Add(new Sensor
                    {
                        Id = s.Id,
                        LocationId = location.Id,
                        Parameter = s.Parameter.Name,
                        DisplayName = s.Parameter.DisplayName,
                        Units = s.Parameter.Units
                    });
                }
            }

            // Small delay to respect OpenAQ rate limits
            await Task.Delay(200);
        }

        await _db.SaveChangesAsync();
        _logger.LogInformation("Station sync complete. {Count} stations processed.", locations.Count);
    }

    // Step 2: Ingest latest readings for all active sensors
    public async Task IngestLatestReadingsAsync()
    {
        _logger.LogInformation("Starting readings ingestion...");

        var activeSensors = await _db.Sensors
            .Include(s => s.Station)
            .Where(s => s.Station.IsActive)
            .ToListAsync();

        var dateTo = DateTime.UtcNow;
        var dateFrom = dateTo.AddHours(-2); // Fetch last 2 hours each run
        var totalInserted = 0;

        foreach (var sensor in activeSensors)
        {
            var measurements = await _openAq.GetMeasurementsAsync(sensor.Id, dateFrom, dateTo);

            foreach (var m in measurements)
            {
                if (m.Period?.DatetimeFrom?.Utc == null) continue;

                var readingTime = m.Period.DatetimeFrom.Utc.Value;

                // Skip if we already have this reading
                var exists = await _db.Readings.AnyAsync(r =>
                    r.SensorId == sensor.Id && r.DatetimeUtc == readingTime);

                if (!exists)
                {
                    _db.Readings.Add(new Reading
                    {
                        SensorId = sensor.Id,
                        LocationId = sensor.LocationId,
                        Value = m.Value,
                        DatetimeUtc = readingTime
                    });
                    totalInserted++;
                }
            }

            await Task.Delay(150); // Rate limit courtesy delay
        }

        await _db.SaveChangesAsync();
        _logger.LogInformation("Readings ingestion complete. {Count} new readings inserted.", totalInserted);
    }
}