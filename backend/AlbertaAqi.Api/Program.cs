using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.PostgreSql;
using AlbertaAqi.Api.Data;
using AlbertaAqi.Api.Services;
using AlbertaAqi.Api.Jobs;
using DotNetEnv;

// Load .env from project root
var envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", ".env");
if (File.Exists(envPath))
{
    Env.Load(envPath);
}

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION")
    ?? throw new InvalidOperationException("POSTGRES_CONNECTION not set.");

// PostgreSQL via EF Core
builder.Services.AddDbContext<AqiDbContext>(options =>
    options.UseNpgsql(connectionString));

// Hangfire with PostgreSQL storage
builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(connectionString));
builder.Services.AddHangfireServer();

// Register OpenAQ HttpClient with API key header
builder.Services.AddHttpClient<OpenAqService>(client =>
{
    var apiKey = Environment.GetEnvironmentVariable("OPENAQ_API_KEY") ?? string.Empty;
    client.DefaultRequestHeaders.Add("X-API-Key", apiKey);
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Register jobs and services
builder.Services.AddScoped<IngestionJob>();

// CORS — allow React frontend (runs on port 5173 via Vite)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowFrontend");

app.UseAuthorization();
app.MapControllers();

// Hangfire dashboard (local dev only)
app.UseHangfireDashboard("/internal-jobs-dashboard", new DashboardOptions
{
    Authorization = new[] { new AllowAllDashboardAuthorizationFilter() }
});

// Schedule recurring jobs
RecurringJob.AddOrUpdate<IngestionJob>(
    "sync-stations",
    job => job.SyncStationsAsync(),
    Cron.Daily);

RecurringJob.AddOrUpdate<IngestionJob>(
    "ingest-readings",
    job => job.IngestLatestReadingsAsync(),
    Cron.Hourly);

app.Run();

public class AllowAllDashboardAuthorizationFilter : Hangfire.Dashboard.IDashboardAuthorizationFilter
{
    public bool Authorize(Hangfire.Dashboard.DashboardContext context) => true;
}