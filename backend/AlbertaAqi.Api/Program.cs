using Microsoft.EntityFrameworkCore;
using AlbertaAqi.Api.Data;
using DotNetEnv;

// Load .env file from project root (two levels up from the running binary)
Env.Load(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", ".env"));

var builder = WebApplication.CreateBuilder(args);

// Register PostgreSQL via EF Core
var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION")
    ?? throw new InvalidOperationException("POSTGRES_CONNECTION environment variable not set.");

builder.Services.AddDbContext<AqiDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();