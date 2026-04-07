namespace AlbertaAqi.Api.DTOs;

// Returned by GET /api/stations
public class StationDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Locality { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsMonitor { get; set; }
    public bool IsActive { get; set; }
    public string? Provider { get; set; }
    public DateTime? DatetimeLast { get; set; }
    public List<SensorDto> Sensors { get; set; } = new();
}

// Nested inside StationDto
public class SensorDto
{
    public int Id { get; set; }
    public string Parameter { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Units { get; set; } = string.Empty;
}

// Returned by GET /api/readings
public class ReadingDto
{
    public int SensorId { get; set; }
    public int LocationId { get; set; }
    public double Value { get; set; }
    public DateTime DatetimeUtc { get; set; }
}