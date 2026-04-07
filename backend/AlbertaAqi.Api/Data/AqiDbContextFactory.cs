using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DotNetEnv;

namespace AlbertaAqi.Api.Data;

public class AqiDbContextFactory : IDesignTimeDbContextFactory<AqiDbContext>
{
    public AqiDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        var envPath = Path.Combine(basePath, ".env");

        if (!File.Exists(envPath))
            envPath = Path.Combine(basePath, "..", "..", ".env");

        Env.Load(envPath);

        var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION")
            ?? throw new InvalidOperationException("POSTGRES_CONNECTION not found in .env");

        var optionsBuilder = new DbContextOptionsBuilder<AqiDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new AqiDbContext(optionsBuilder.Options);
    }
}