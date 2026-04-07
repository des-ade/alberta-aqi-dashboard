using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlbertaAqi.Api.Models;

[Table("stations")]
public class Station
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("locality")]
    public string? Locality { get; set; }

    [Column("timezone")]
    public string Timezone { get; set; } = string.Empty;

    [Column("latitude")]
    public double Latitude { get; set; }

    [Column("longitude")]
    public double Longitude { get; set; }

    [Column("is_monitor")]
    public bool IsMonitor { get; set; }

    [Column("provider")]
    public string? Provider { get; set; }

    [Column("datetime_first")]
    public DateTime? DatetimeFirst { get; set; }

    [Column("datetime_last")]
    public DateTime? DatetimeLast { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    // Navigation property
    public ICollection<Sensor> Sensors { get; set; } = new List<Sensor>();
}