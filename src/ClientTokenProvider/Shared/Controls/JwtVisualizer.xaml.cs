using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Shared.Models;

namespace ClientTokenProvider.Shared.Controls;

public partial class JwtVisualizer : ContentView
{
    public static readonly BindableProperty JwtTokenProperty = BindableProperty.Create(
        nameof(JwtToken),
        typeof(string),
        typeof(JwtVisualizer));

    public static readonly BindableProperty DecodedJwtTokenProperty = BindableProperty.Create(
        nameof(DecodedJwtToken),
        typeof(DecodedJwt),
        typeof(JwtVisualizer));

    public static readonly BindableProperty ModeProperty = BindableProperty.Create(
       nameof(Mode),
       typeof(JwtVisualizerMode),
       typeof(JwtVisualizer));

    public string JwtToken
    {
        get => (string)GetValue(JwtTokenProperty);
        set => SetValue(JwtTokenProperty, value);
    }

    public DecodedJwt DecodedJwtToken
    {
        get => (DecodedJwt)GetValue(DecodedJwtTokenProperty);
        set => SetValue(DecodedJwtTokenProperty, value);
    }

    public JwtVisualizerMode Mode
    {
        get => (JwtVisualizerMode)GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }

    public JwtVisualizer()
    {
        InitializeComponent();
    }
}