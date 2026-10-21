using KittyToken.Application.Shared.JwtDecoding;
using KittyToken.Maui.Models;

namespace KittyToken.Maui.Controls;

public sealed partial class JwtVisualizer :
    ContentView
{
    public static readonly BindableProperty TokenProperty = BindableProperty.Create(
        nameof(Token),
        typeof(string),
        typeof(JwtVisualizer),
        defaultValue: string.Empty);

    public static readonly BindableProperty DecodedTokenProperty = BindableProperty.Create(
        nameof(DecodedToken),
        typeof(DecodedJwt),
        typeof(JwtVisualizer));

    public static readonly BindableProperty VisualizationModeProperty = BindableProperty.Create(
        nameof(VisualizationMode),
        typeof(JwtVisualizationMode),
        typeof(JwtVisualizer),
        defaultValue: JwtVisualizationMode.None,
        defaultBindingMode: BindingMode.TwoWay);

    public string Token
    {
        get => (string)GetValue(TokenProperty);
        set => SetValue(TokenProperty, value);
    }

    public DecodedJwt? DecodedToken
    {
        get => (DecodedJwt?)GetValue(DecodedTokenProperty);
        set => SetValue(DecodedTokenProperty, value);
    }

    public JwtVisualizationMode VisualizationMode
    {
        get => (JwtVisualizationMode)GetValue(VisualizationModeProperty);
        set => SetValue(VisualizationModeProperty, value);
    }

    public IReadOnlyList<JwtVisualizationMode> VisualizationModes { get; } = Enum.GetValues<JwtVisualizationMode>();

    public JwtVisualizer()
    {
        InitializeComponent();
    }
}
