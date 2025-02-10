using ClientTokenProvider.AzureAd.Extensions;
using ClientTokenProvider.Business.AzureAd.Extensions;
using ClientTokenProvider.Business.Shared.Extensions;
using ClientTokenProvider.Core.AzureAd.Extensions;
using ClientTokenProvider.Persistence.Shared.Extensions;
using ClientTokenProvider.Shared.Extensions;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace ClientTokenProvider;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        var services = builder.Services;

        builder
          .UseMauiCommunityToolkit();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        var isDevelopment = false;

#if DEBUG
        isDevelopment = true;

        builder.Logging.AddDebug();
#endif

        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var dbPath = System.IO.Path.Join(path, "Configurations.sqlite3");

        var dbConnectionString = $"Data Source={dbPath}";

        services
            .AddClientTokenProviderBusinessShared()
            .AddClientTokenProviderPersistenceShared(
                dbConnectionString,
                isDevelopment)
            .AddClientTokenProviderShared();

        services
            .AddClientTokenProviderCoreAzureAd()
            .AddClientTokenProviderBusinessAzureAd()
            .AddClientTokenProviderAzureAd();

        return builder.Build();
    }
}
