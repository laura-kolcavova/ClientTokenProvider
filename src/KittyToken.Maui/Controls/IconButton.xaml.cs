namespace KittyToken.Maui.Controls;

public sealed partial class IconButton :
    ContentView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(IconButton),
        defaultValue: string.Empty);

    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
        nameof(ImageSource),
        typeof(ImageSource),
        typeof(IconButton));

    public event EventHandler? Clicked;

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public ImageSource? ImageSource
    {
        get => (ImageSource?)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    public IconButton()
    {
        InitializeComponent();
    }

    private void TapGestureRecognizer_Tapped(
        object? sender,
        TappedEventArgs e)
    {
        Clicked?.Invoke(this, EventArgs.Empty);
    }
}
