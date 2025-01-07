using ClientTokenProvider.Shared.Services;
using ClientTokenProvider.Shared.ViewModels;
using ClientTokenProvider.Shared.Views;
using CommunityToolkit.Maui;

namespace ClientTokenProvider.Shared.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientTokenProviderShared(
        this IServiceCollection services)
    {
        services
            .AddSingleton<INavigationService, NavigationService>();

        services
            .AddTransient<ConfigurationListView, ConfigurationListViewModel>()
            .AddTransient<ConfigurationManagerPage, ConfigurationManagerViewModel>();

        Routes.RegisterRoutes();

        return services;
    }
}
