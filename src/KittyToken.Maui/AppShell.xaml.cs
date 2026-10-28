namespace KittyToken.Maui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

#if DEBUG
        Items.Add(new ShellContent
        {
            Title = "JwtVisualizer test",
            Route = nameof(Views.JwtVisualizerTestPage),
            ContentTemplate = new DataTemplate(typeof(Views.JwtVisualizerTestPage)),
        });
#endif
    }
}
