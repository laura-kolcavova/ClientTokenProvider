using KittyToken.Application.Shared.JwtDecoding;
using KittyToken.Application.Shared.JwtDecoding.Abstractions;

namespace KittyToken.Maui.Views;

public sealed partial class DecodeJwtPage : ContentPage
{
    private const string SampleToken =
        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9" +
        ".eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ" +
        ".SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";

    private readonly IJwtDecoder _jwtDecoder;

    private string _token = string.Empty;

    private DecodedJwt? _decodedToken;

    public string Token
    {
        get => _token;
        set
        {
            _token = value;
            OnPropertyChanged();

            DecodedToken = _jwtDecoder.Decode(value);
        }
    }

    public DecodedJwt? DecodedToken
    {
        get => _decodedToken;
        private set
        {
            _decodedToken = value;
            OnPropertyChanged();
        }
    }

    public DecodeJwtPage(
        IJwtDecoder jwtDecoder)
    {
        _jwtDecoder = jwtDecoder;

        InitializeComponent();

        BindingContext = this;

        Token = SampleToken;
    }
}
