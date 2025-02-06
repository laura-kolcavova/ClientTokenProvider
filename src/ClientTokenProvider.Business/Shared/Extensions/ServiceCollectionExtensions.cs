using ClientTokenProvider.Business.Persistence.Shared;
using ClientTokenProvider.Business.Persistence.Shared.Services;
using ClientTokenProvider.Business.Shared.Services;
using ClientTokenProvider.Business.Shared.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace ClientTokenProvider.Business.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientTokenProviderBusinessShared(
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
            .AddSingleton<IConfigurationFactory, ConfigurationFactory>()
            .AddSingleton<IClientTokenProviderFactory, ClientTokenProviderFactory>();

        services
            .AddScoped<IConfigurationRepository, ConfigurationRepositorySqlLite>();

        return services;
    }
}
