using ClientTokenProvider.AzureAd.Managers;
using ClientTokenProvider.AzureAd.ViewModels;
using ClientTokenProvider.AzureAd.Views;
using ClientTokenProvider.Core.AzureAd.Extensions;
using CommunityToolkit.Maui;

namespace ClientTokenProvider.AzureAd.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientTokenProviderApplication(
        this IServiceCollection services)
    {
        services
            .AddTransient<ConfigurationDetailView, ConfigurationDetailViewModel>();

        services
            .AddAzureAdClientTokenProvider();

        services
            .AddSingleton<IConfigurationFileManager, ConfigurationFileManager>();

        return services;
    }
}
