using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlbertaAqi.Api.Data;
using AlbertaAqi.Api.DTOs;

namespace AlbertaAqi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReadingsController : ControllerBase
{
    private readonly AqiDbContext _db;

    public ReadingsController(AqiDbContext db)
    {
        _db = db;
    }

    // GET /api/readings?sensorId=123&from=2024-01-01&to=2024-01-07
    [HttpGet]
    public async Task<ActionResult<List<ReadingDto>>> GetReadings(
        [FromQuery] int sensorId,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null)
    {
        var dateFrom = from ?? DateTime.UtcNow.AddDays(-7);
        var dateTo = to ?? DateTime.UtcNow;

        var readings = await _db.Readings
            .Where(r => r.SensorId == sensorId &&
                        r.DatetimeUtc >= dateFrom &&
                        r.DatetimeUtc <= dateTo)
            .OrderBy(r => r.DatetimeUtc)
            .Select(r => new ReadingDto
            {
                SensorId = r.SensorId,
                LocationId = r.LocationId,
                Value = r.Value,
                DatetimeUtc = r.DatetimeUtc
            })
            .ToListAsync();

        return Ok(readings);
    }
}