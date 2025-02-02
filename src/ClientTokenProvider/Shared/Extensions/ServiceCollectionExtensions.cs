using ClientTokenProvider.Shared.Services;
using ClientTokenProvider.Shared.Services.Abstractions;
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
            .AddTransient<ConfigurationManagerPage, ConfigurationManagerViewModel>();

        services
            .AddSingleton<IConfigurationDataMapper, ConfigurationDataMapper>();

        return services;
    }
}
