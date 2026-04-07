namespace AlbertaAqi.Api.DTOs;

// --- Locations endpoint DTOs ---

public class OpenAqLocationResponse
{
    public List<OpenAqLocation> Results { get; set; } = new();
}

public class OpenAqLocation
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Locality { get; set; }
    public string Timezone { get; set; } = string.Empty;
    public bool IsMonitor { get; set; }
    public OpenAqCoordinates? Coordinates { get; set; }
    public OpenAqProvider? Provider { get; set; }
    public OpenAqDatetime? DatetimeFirst { get; set; }
    public OpenAqDatetime? DatetimeLast { get; set; }
    public List<OpenAqSensor> Sensors { get; set; } = new();
}

public class OpenAqCoordinates
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class OpenAqProvider
{
    public string Name { get; set; } = string.Empty;
}

public class OpenAqDatetime
{
    public DateTime? Utc { get; set; }
}

// --- Sensors endpoint DTOs ---

public class OpenAqSensorsResponse
{
    public List<OpenAqSensor> Results { get; set; } = new();
}

public class OpenAqSensor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public OpenAqParameter? Parameter { get; set; }
}

public class OpenAqParameter
{
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Units { get; set; } = string.Empty;
}

// --- Measurements endpoint DTOs ---

public class OpenAqMeasurementsResponse
{
    public List<OpenAqMeasurement> Results { get; set; } = new();
}

public class OpenAqMeasurement
{
    public double Value { get; set; }
    public OpenAqPeriod? Period { get; set; }
}

public class OpenAqPeriod
{
    public OpenAqDatetime? DatetimeFrom { get; set; }
}