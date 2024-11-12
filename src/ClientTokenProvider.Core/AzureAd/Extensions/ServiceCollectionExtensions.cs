using ClientTokenProvider.Core.AzureAd.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace ClientTokenProvider.Core.AzureAd.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientTokenProviderCoreAzureAd(
        this IServiceCollection services)
    {
        services
            .AddHttpClient();

        services
            .AddScoped<IAzureAdClientTokenProviderFactory, AzureAdClientTokenProviderFactory>();

        return services;
    }
}
