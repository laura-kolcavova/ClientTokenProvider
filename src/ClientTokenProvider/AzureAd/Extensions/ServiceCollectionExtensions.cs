using ClientTokenProvider.AzureAd.ViewModels;
using ClientTokenProvider.AzureAd.Views;
using CommunityToolkit.Maui;

namespace ClientTokenProvider.AzureAd.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientTokenProviderAzureAd(
        this IServiceCollection services)
    {
        services
            .AddTransient<ConfigurationDetailView, ConfigurationDetailViewModel>();

        return services;
    }
}
