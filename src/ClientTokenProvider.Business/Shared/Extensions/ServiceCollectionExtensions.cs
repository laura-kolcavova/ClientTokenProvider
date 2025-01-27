using ClientTokenProvider.Business.Shared.Services;
using ClientTokenProvider.Business.Shared.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace ClientTokenProvider.Business.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientTokenProviderBusinessShared(
        this IServiceCollection services)
    {
        services
            .AddMemoryCache();

        services
            .AddSingleton<IConfigurationActionStateStore, ConfigurationActionStateStore>()
            .AddSingleton<IConfigurationFactory, ConfigurationFactory>()
            .AddSingleton<IConfigurationCacheService, ConfigurationCacheService>();

        services
            .AddScoped<IConfigurationService, ConfigurationService>();

        services
            .AddScoped<IConfigurationRepository, ConfigurationRepositoryMock>();

        return services;
    }
}
