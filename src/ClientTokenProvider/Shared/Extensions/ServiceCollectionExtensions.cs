using ClientTokenProvider.Shared.Services;
using ClientTokenProvider.Shared.Services.Abstractions;
using ClientTokenProvider.Shared.ViewModels;
using ClientTokenProvider.Shared.Views;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;

namespace ClientTokenProvider.Shared.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientTokenProviderShared(
        this IServiceCollection services)
    {
        services
            .AddTransient<ConfigurationManagerPage, ConfigurationManagerViewModel>();

        services
            .AddSingleton<IConfigurationDataMapper, ConfigurationDataMapper>()
            .AddSingleton<IConfigurationDataBackupStore, ConfigurationDataBackupStore>()
            .AddSingleton<IConfigurationExporter, ConfigurationExporter>();

        services
            .AddSingleton<IFileSaver>(FileSaver.Default);

        return services;
    }
}
