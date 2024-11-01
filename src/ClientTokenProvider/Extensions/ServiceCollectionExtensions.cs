using ClientTokenProvider.AzureAd.ViewModels;
using ClientTokenProvider.AzureAd.Views;
using ClientTokenProvider.Core.AzureAd.Extensions;
using CommunityToolkit.Maui;

namespace ClientTokenProvider.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientTokenProviderApplication(
        this IServiceCollection services)
    {
        services
            .AddTransient<ConfigurationDetailView, ConfigurationDetailViewModel>();

        services
            .AddAzureAdClientTokenProvider();

        return services;
    }
}
