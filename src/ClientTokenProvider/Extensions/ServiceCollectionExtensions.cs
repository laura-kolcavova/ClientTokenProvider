using ClientTokenProvider.AzureAd.ViewModels;
using ClientTokenProvider.AzureAd.Views;
using CommunityToolkit.Maui;

namespace ClientTokenProvider.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientTokenProvider(
        this IServiceCollection services)
    {
        services
            .AddTransient<ConfigurationDetailView, ConfigurationDetailViewModel>();

        return services;
    }
}
