using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlbertaAqi.Api.Data;
using AlbertaAqi.Api.DTOs;

namespace AlbertaAqi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StationsController : ControllerBase
{
    private readonly AqiDbContext _db;

    public StationsController(AqiDbContext db)
    {
        _db = db;
    }

    // GET /api/stations — all Alberta stations with their sensors
    [HttpGet]
    public async Task<ActionResult<List<StationDto>>> GetStations(
        [FromQuery] bool activeOnly = false)
    {
        var query = _db.Stations
            .Include(s => s.Sensors)
            .AsQueryable();

        if (activeOnly)
            query = query.Where(s => s.IsActive);

        var stations = await query.ToListAsync();

        var result = stations.Select(s => new StationDto
        {
            Id = s.Id,
            Name = s.Name,
            Locality = s.Locality,
            Latitude = s.Latitude,
            Longitude = s.Longitude,
            IsMonitor = s.IsMonitor,
            IsActive = s.IsActive,
            Provider = s.Provider,
            DatetimeLast = s.DatetimeLast,
            Sensors = s.Sensors.Select(sensor => new SensorDto
            {
                Id = sensor.Id,
                Parameter = sensor.Parameter,
                DisplayName = sensor.DisplayName,
                Units = sensor.Units
            }).ToList()
        }).ToList();

        return Ok(result);
    }

    // GET /api/stations/{id} — single station
    [HttpGet("{id}")]
    public async Task<ActionResult<StationDto>> GetStation(int id)
    {
        var station = await _db.Stations
            .Include(s => s.Sensors)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (station == null) return NotFound();

        return Ok(new StationDto
        {
            Id = station.Id,
            Name = station.Name,
            Locality = station.Locality,
            Latitude = station.Latitude,
            Longitude = station.Longitude,
            IsMonitor = station.IsMonitor,
            IsActive = station.IsActive,
            Provider = station.Provider,
            DatetimeLast = station.DatetimeLast,
            Sensors = station.Sensors.Select(s => new SensorDto
            {
                Id = s.Id,
                Parameter = s.Parameter,
                DisplayName = s.DisplayName,
                Units = s.Units
            }).ToList()
        });
    }

    // GET /api/stations/latest-pm25
    [HttpGet("latest-pm25")]
    public async Task<ActionResult<Dictionary<int, double>>> GetLatestPm25()
    {
        var latest = await _db.Readings
            .Where(r => r.Sensor.Parameter == "pm25")
            .GroupBy(r => r.LocationId)
            .Select(g => new
            {
                LocationId = g.Key,
                Value = g.OrderByDescending(r => r.DatetimeUtc)
                        .First().Value
            })
            .ToDictionaryAsync(x => x.LocationId, x => x.Value);

        return Ok(latest);
    }
}