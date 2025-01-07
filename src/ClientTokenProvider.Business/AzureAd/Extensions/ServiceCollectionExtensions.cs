using ClientTokenProvider.Business.AzureAd.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace ClientTokenProvider.Business.AzureAd.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientTokenProviderBusinessAzureAd(
        this IServiceCollection services)
    {
        services
          .AddSingleton<IAzureAdConfigurationFactory, AzureAdConfigurationFactory>();

        return services;
    }
}