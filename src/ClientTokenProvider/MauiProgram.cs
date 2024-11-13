using ClientTokenProvider.AzureAd.Extensions;
using ClientTokenProvider.Business.Shared.Extensions;
using ClientTokenProvider.Core.AzureAd.Extensions;
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

#if DEBUG
        builder.Logging.AddDebug();
#endif

        services
            .AddClientTokenProviderBusinessShared()
            .AddClientTokenProviderShared();

        services
            .AddClientTokenProviderCoreAzureAd()
            .AddClientTokenProviderAzureAd();

        return builder.Build();
    }
}
