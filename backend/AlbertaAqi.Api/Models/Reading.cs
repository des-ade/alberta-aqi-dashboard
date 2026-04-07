using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlbertaAqi.Api.Models;

[Table("readings")]
public class Reading
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("sensor_id")]
    public int SensorId { get; set; }

    [Column("location_id")]
    public int LocationId { get; set; }

    [Column("value")]
    public double Value { get; set; }

    [Column("datetime_utc")]
    public DateTime DatetimeUtc { get; set; }

    // Navigation property
    [ForeignKey("SensorId")]
    public Sensor Sensor { get; set; } = null!;
}