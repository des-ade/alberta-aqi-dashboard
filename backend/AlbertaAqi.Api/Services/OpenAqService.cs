using System.Text.Json;
using AlbertaAqi.Api.DTOs;

namespace AlbertaAqi.Api.Services;

public class OpenAqService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OpenAqService> _logger;

    // Alberta bounding box confirmed working in Phase 2
    private const string AlbertaBbox = "-120.001343,48.994,-109.994,60.002";
    private const string BaseUrl = "https://api.openaq.org/v3";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public OpenAqService(HttpClient httpClient, ILogger<OpenAqService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    // Fetch all stations within Alberta bounding box
    public async Task<List<OpenAqLocation>> GetAlbertaStationsAsync()
    {
        try
        {
            var url = $"{BaseUrl}/locations?bbox={AlbertaBbox}&limit=100";
            var response = await _httpClient.GetStringAsync(url);
            var result = JsonSerializer.Deserialize<OpenAqLocationResponse>(response, JsonOptions);
            var locations = result?.Results ?? new List<OpenAqLocation>();

            // Filter to confirmed Alberta stations using timezone
            // locality is unreliable — some are null, N/A, or incorrectly set
            var albertaStations = locations
                .Where(l =>
                    l.Timezone == "America/Edmonton" &&
                    l.Coordinates != null &&
                    l.Locality != "BRITISH COLUMBIA")
                .ToList();

            _logger.LogInformation("Fetched {Count} Alberta stations from OpenAQ", albertaStations.Count);
            return albertaStations;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch Alberta stations from OpenAQ");
            return new List<OpenAqLocation>();
        }
    }

    // Fetch all sensors for a specific station
    public async Task<List<OpenAqSensor>> GetSensorsForStationAsync(int locationId)
    {
        try
        {
            var url = $"{BaseUrl}/locations/{locationId}/sensors";
            var response = await _httpClient.GetStringAsync(url);
            var result = JsonSerializer.Deserialize<OpenAqSensorsResponse>(response, JsonOptions);
            return result?.Results ?? new List<OpenAqSensor>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch sensors for station {LocationId}", locationId);
            return new List<OpenAqSensor>();
        }
    }

    // Fetch historical measurements for a specific sensor
    public async Task<List<OpenAqMeasurement>> GetMeasurementsAsync(
        int sensorId, DateTime dateFrom, DateTime dateTo)
    {
        try
        {
            var from = dateFrom.ToString("yyyy-MM-ddTHH:mm:ssZ");
            var to = dateTo.ToString("yyyy-MM-ddTHH:mm:ssZ");
            var url = $"{BaseUrl}/sensors/{sensorId}/measurements?date_from={from}&date_to={to}&limit=1000";

            var response = await _httpClient.GetStringAsync(url);
            var result = JsonSerializer.Deserialize<OpenAqMeasurementsResponse>(response, JsonOptions);
            return result?.Results ?? new List<OpenAqMeasurement>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch measurements for sensor {SensorId}", sensorId);
            return new List<OpenAqMeasurement>();
        }
    }
}