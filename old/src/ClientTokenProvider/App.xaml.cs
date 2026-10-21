using ClientTokenProvider.Persistence.Shared.Services.Abstractions;

namespace ClientTokenProvider;

public partial class App : Application
{
    public App(
        IDatabaseInitializer databaseInitializer)
    {
        InitializeComponent();

        UserAppTheme = AppTheme.Dark;
        MainPage = new AppShell();

        databaseInitializer.Initialize();
    }

    private void SetUpEntry()
    {
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("SetUpEntry", (handler, view) =>
        {
#if ANDROID

#elif IOS || MACCATALYST

            handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;

#elif WINDOWS
#endif
        });
    }
}
