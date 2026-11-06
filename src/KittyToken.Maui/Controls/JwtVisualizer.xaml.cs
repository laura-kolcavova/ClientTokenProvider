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
        defaultValue: string.Empty,
        propertyChanged: OnTokenPropertyChanged);

    public static readonly BindableProperty DecodedTokenProperty = BindableProperty.Create(
        nameof(DecodedToken),
        typeof(DecodedJwt),
        typeof(JwtVisualizer));

    public static readonly BindableProperty VisualizationModeProperty = BindableProperty.Create(
        nameof(VisualizationMode),
        typeof(JwtVisualizationMode),
        typeof(JwtVisualizer),
        defaultValue: JwtVisualizationMode.Encoded,
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: OnVisualizationModePropertyChanged);

    private string _encodedHeader = string.Empty;

    private string _encodedPayload = string.Empty;

    private string _encodedSignature = string.Empty;

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

    public bool IsEncodedVisible => VisualizationMode == JwtVisualizationMode.Encoded;

    public bool IsDecodedVisible => VisualizationMode == JwtVisualizationMode.Decoded;

    public string HeaderSeparator => EncodedPayload.Length > 0 ? "." : string.Empty;

    public string PayloadSeparator => EncodedSignature.Length > 0 ? "." : string.Empty;

    public string EncodedHeader
    {
        get => _encodedHeader;
        private set
        {
            _encodedHeader = value;
            OnPropertyChanged();
        }
    }

    public string EncodedPayload
    {
        get => _encodedPayload;
        private set
        {
            _encodedPayload = value;
            OnPropertyChanged();
        }
    }

    public string EncodedSignature
    {
        get => _encodedSignature;
        private set
        {
            _encodedSignature = value;
            OnPropertyChanged();
        }
    }

    public JwtVisualizer()
    {
        InitializeComponent();
    }

    private static void OnTokenPropertyChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var self = (JwtVisualizer)bindable;

        var parts = ((string?)newValue ?? string.Empty).Split('.');

        self.EncodedHeader = parts.ElementAtOrDefault(0) ?? string.Empty;
        self.EncodedPayload = parts.ElementAtOrDefault(1) ?? string.Empty;
        self.EncodedSignature = parts.ElementAtOrDefault(2) ?? string.Empty;

        self.OnPropertyChanged(nameof(HeaderSeparator));
        self.OnPropertyChanged(nameof(PayloadSeparator));
    }

    private static void OnVisualizationModePropertyChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var self = (JwtVisualizer)bindable;

        self.OnPropertyChanged(nameof(IsEncodedVisible));
        self.OnPropertyChanged(nameof(IsDecodedVisible));
    }
}
