using KittyToken.Application.AzureAd.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KittyToken.Application.AzureAd.Persistence;

internal sealed class AzureAdFileDbContext(
    string connectionString,
    bool useDevelopmentLogging) :
    DbContext
{
    public DbSet<AzureAdFile> AzureAdFiles => Set<AzureAdFile>();

    public void Init()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
           .UseSqlite(connectionString);

        if (useDevelopmentLogging)
        {
            optionsBuilder
                .UseLoggerFactory(CreateLoggerFactory())
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AzureAdFileEntityConfiguration());
    }

    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder =>
            builder.AddConsole());
    }
}
