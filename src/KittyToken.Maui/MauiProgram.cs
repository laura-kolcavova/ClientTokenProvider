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

        var isDevelopment = false;

#if DEBUG
        isDevelopment = true;
#endif

        var dbConnectionString = GetDbConnectionString();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services
            .AddApplication(dbConnectionString, isDevelopment);

        builder.Services
            .AddTransient<MainPage>()
            .AddTransient<JwtVisualizerTestPage>();

        if (isDevelopment)
        {
            builder.Logging.AddDebug();
        }
        return builder.Build();
    }

    private static string GetDbConnectionString()
    {

        var appDataFolder = Environment.SpecialFolder.LocalApplicationData;
        var appDataPath = Environment.GetFolderPath(appDataFolder);
        var dbFilePath = Path.Join(appDataPath, "KittyToken.sqlite3");

        return $"Data Source={dbFilePath}";
    }
}
