using ClientTokenProvider.AzureAd.Extensions;
using ClientTokenProvider.Business.AzureAd.Extensions;
using ClientTokenProvider.Business.Shared.Extensions;
using ClientTokenProvider.Core.AzureAd.Extensions;
using ClientTokenProvider.Persistence.Shared.Extensions;
using ClientTokenProvider.Shared.Extensions;
using ClientTokenProvider.Shared.Messages;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

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
            })
            .ConfigureLifecycleEvents(events =>
            {
#if WINDOWS
                events.AddWindows(windowsLifecycleBuilder =>
                {
                    windowsLifecycleBuilder.OnWindowCreated(window =>
                    {
                        //use Microsoft.UI.Windowing functions for window
                        var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                        var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                        var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);

                        //When user execute the closing method, we can push a display alert. If user click Yes, close this application, if click the cancel, display alert will dismiss.
                        appWindow.Closing += (s, e) =>
                        {
                            e.Cancel = true;

                            WeakReferenceMessenger.Default.Send(
                                new CloseAppMessage());
                        };
                    });
                });
#endif
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
