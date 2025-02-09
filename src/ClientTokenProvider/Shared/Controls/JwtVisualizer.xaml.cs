using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Shared.Models;

namespace ClientTokenProvider.Shared.Controls;

public partial class JwtVisualizer : ContentView
{
    public static readonly BindableProperty TokenProperty = BindableProperty.Create(
        nameof(Token),
        typeof(string),
        typeof(JwtVisualizer));

    public static readonly BindableProperty DecodedTokenProperty = BindableProperty.Create(
        nameof(DecodedToken),
        typeof(DecodedJwt),
        typeof(JwtVisualizer));

    public static readonly BindableProperty VisualizationModeProperty = BindableProperty.Create(
       nameof(VisualizationMode),
       typeof(JwtVisualizationMode),
       typeof(JwtVisualizer));

    public string Token
    {
        get => (string)GetValue(TokenProperty);
        set => SetValue(TokenProperty, value);
    }

    public DecodedJwt DecodedToken
    {
        get => (DecodedJwt)GetValue(DecodedTokenProperty);
        set => SetValue(DecodedTokenProperty, value);
    }

    public JwtVisualizationMode VisualizationMode
    {
        get => (JwtVisualizationMode)GetValue(VisualizationModeProperty);
        set => SetValue(VisualizationModeProperty, value);
    }

    public List<JwtVisualizationMode> VisualizationModes { get; } = Enum
        .GetValues(typeof(JwtVisualizationMode))
        .Cast<JwtVisualizationMode>()
        .ToList();

    public JwtVisualizer()
    {
        InitializeComponent();
    }
}