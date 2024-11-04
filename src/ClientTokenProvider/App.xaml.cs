namespace ClientTokenProvider;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        UserAppTheme = AppTheme.Dark;
        MainPage = new AppShell();
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
