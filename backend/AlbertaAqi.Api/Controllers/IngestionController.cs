using Microsoft.AspNetCore.Mvc;
using AlbertaAqi.Api.Jobs;

namespace AlbertaAqi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngestionController : ControllerBase
{
    private readonly IngestionJob _job;

    public IngestionController(IngestionJob job)
    {
        _job = job;
    }

    // POST /api/ingestion/sync-stations
    [HttpPost("sync-stations")]
    public async Task<IActionResult> SyncStations()
    {
        await _job.SyncStationsAsync();
        return Ok(new { message = "Station sync complete." });
    }

    // POST /api/ingestion/ingest-readings
    [HttpPost("ingest-readings")]
    public async Task<IActionResult> IngestReadings()
    {
        await _job.IngestLatestReadingsAsync();
        return Ok(new { message = "Readings ingestion complete." });
    }
}