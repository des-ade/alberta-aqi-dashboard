using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlbertaAqi.Api.Models;

[Table("sensors")]
public class Sensor
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("location_id")]
    public int LocationId { get; set; }

    [Column("parameter")]
    public string Parameter { get; set; } = string.Empty;

    [Column("display_name")]
    public string DisplayName { get; set; } = string.Empty;

    [Column("units")]
    public string Units { get; set; } = string.Empty;

    // Navigation properties
    [ForeignKey("LocationId")]
    public Station Station { get; set; } = null!;

    public ICollection<Reading> Readings { get; set; } = new List<Reading>();
}