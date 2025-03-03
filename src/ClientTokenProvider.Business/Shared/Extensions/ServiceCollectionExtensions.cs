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
            .AddSingleton<IConfigurationFactory, ConfigurationFactory>()
            .AddSingleton<IConfigurationExporter, ConfigurationExporter>()
            .AddSingleton<IClientTokenProviderFactory, ClientTokenProviderFactory>()
            .AddSingleton<IJwtDecoder, JwtDecoder>();

        return services;
    }
}
