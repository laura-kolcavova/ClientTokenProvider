using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Persistence.Shared.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClientTokenProvider.Persistence.Shared;

internal sealed class ConfigurationDbContext :
    DbContext
{
    private readonly string _connectionString;
    private readonly bool _useDevelopmentLogging;

    public DbSet<ConfigurationModel> Configurations => Set<ConfigurationModel>();

    public ConfigurationDbContext(
        string connectionString,
        bool useDevelopmentLogging)
    {
        _connectionString = connectionString;
        _useDevelopmentLogging = useDevelopmentLogging;
    }

    public void Init()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
           .UseSqlite(_connectionString);

        if (_useDevelopmentLogging)
        {
            optionsBuilder
                .UseLoggerFactory(CreateLoggerFactory())
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ConfigurationEntityTypeConfiguration());
    }

    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder =>
            builder.AddConsole());
    }
}
