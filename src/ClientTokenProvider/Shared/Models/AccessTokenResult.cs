using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Shared.Models;

public sealed record AccessTokenResult
{
    public AccessTokenResultState State { get; }

    public string Token { get; }

    public DecodedJwt DecodedToken { get; }

    public string ErrorMessage { get; }

    private AccessTokenResult(
        AccessTokenResultState state,
        string token,
        DecodedJwt decodedJwt,
        string errorMessage)
    {
        State = state;
        Token = token;
        DecodedToken = decodedJwt;
        ErrorMessage = errorMessage;
    }

    public static AccessTokenResult None() =>
       new(
           AccessTokenResultState.None,
           string.Empty,
           EmptyDecodedJwt(),
           string.Empty);

    public static AccessTokenResult Succeeded(
        string token,
        DecodedJwt decodedJwt) =>
        new(
            AccessTokenResultState.Succeeded,
            token,
            decodedJwt,
            string.Empty);

    public static AccessTokenResult Failed(string errorMessage) =>
        new(
            AccessTokenResultState.Failed,
            string.Empty,
            EmptyDecodedJwt(),
            errorMessage);

    private static DecodedJwt EmptyDecodedJwt() => new DecodedJwt();
}
