using KittyToken.Application.Shared.JwtDecoding;

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

        var parts = ((string)newValue).Split('.');

        self.EncodedHeader = parts.ElementAtOrDefault(0) ?? string.Empty;
        self.EncodedPayload = parts.ElementAtOrDefault(1) ?? string.Empty;
        self.EncodedSignature = parts.ElementAtOrDefault(2) ?? string.Empty;
    }
}
