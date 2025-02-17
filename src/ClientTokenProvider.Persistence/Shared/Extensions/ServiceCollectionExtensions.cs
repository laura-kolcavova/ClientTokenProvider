using ClientTokenProvider.Business.Shared.Services.Abstractions;
using ClientTokenProvider.Persistence.Shared.Services;
using ClientTokenProvider.Persistence.Shared.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace ClientTokenProvider.Persistence.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientTokenProviderPersistenceShared(
        this IServiceCollection services,
        string dbConnectionString,
        bool isDevelopment)
    {
        services
            .AddMemoryCache();

        services
            .AddScoped(serviceProvider =>
            {
                return new ConfigurationDbContext(
                    connectionString: dbConnectionString,
                    useDevelopmentLogging: isDevelopment);
            });

        services
            .AddScoped<IDatabaseInitializer, DatabaseInitializer>()
            .AddScoped<IConfigurationRepository, ConfigurationRepositorySqLite>();

        return services;
    }
}
