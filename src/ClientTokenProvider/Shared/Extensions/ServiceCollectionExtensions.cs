using ClientTokenProvider.Shared.Managers;
using ClientTokenProvider.Shared.ViewModels;
using ClientTokenProvider.Shared.Views;
using CommunityToolkit.Maui;

namespace ClientTokenProvider.Shared.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSharedClientTokenProvider(
        this IServiceCollection services)
    {
        services
            .AddSingleton<IConfigurationIdentityManager, ConfigurationIdentityManager>();

        services
            .AddTransient<ConfigurationListView, ConfigurationListViewModel>();

        return services;
    }
}
