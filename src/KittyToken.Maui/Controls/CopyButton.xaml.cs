namespace KittyToken.Maui.Controls;

public sealed partial class CopyButton :
    ContentView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(CopyButton),
        defaultValue: string.Empty);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public CopyButton()
    {
        InitializeComponent();
    }

    private async void ImageButton_Clicked(
        object? sender,
        EventArgs e)
    {
        await Clipboard.Default.SetTextAsync(Text);

        ImageButton.Source = "check_square.png";

        await Task.Delay(TimeSpan.FromSeconds(1.5));

        ImageButton.Source = "copy.png";
    }
}
