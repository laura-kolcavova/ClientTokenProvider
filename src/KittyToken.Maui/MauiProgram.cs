using CommunityToolkit.Maui;
using KittyToken.Application.Shared.Extensions;
using KittyToken.Maui.Views;
using Microsoft.Extensions.Logging;

namespace KittyToken.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services
            .AddApplication();

        builder.Services
            .AddTransient<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();

        builder.Services
            .AddTransient<JwtVisualizerTestPage>();
#endif

        return builder.Build();
    }
}
