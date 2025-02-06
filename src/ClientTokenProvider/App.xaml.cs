using ClientTokenProvider.Business.Persistence;

namespace ClientTokenProvider;

public partial class App : Application
{
    public App(ConfigurationDbContext configurationDbContext)
    {
        InitializeComponent();

        UserAppTheme = AppTheme.Dark;
        MainPage = new AppShell();

        configurationDbContext.Init();
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
