using ClientTokenProvider.AzureAd.Managers;
using ClientTokenProvider.AzureAd.ViewModels;
using ClientTokenProvider.AzureAd.Views;
using ClientTokenProvider.Core.AzureAd.Extensions;
using CommunityToolkit.Maui;

namespace ClientTokenProvider.AzureAd.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAzureAdClientTokenProvider(
        this IServiceCollection services)
    {
        services
           .AddCoreAzureAdClientTokenProvider();

        services
            .AddTransient<ConfigurationDetailView, ConfigurationDetailViewModel>();

        services
            .AddSingleton<IAzureAdConfigurationManager, AzureAdConfigurationManager>();

        return services;
    }
}
