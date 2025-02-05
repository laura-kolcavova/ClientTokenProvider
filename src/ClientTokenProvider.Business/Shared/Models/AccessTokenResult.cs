namespace ClientTokenProvider.Business.Shared.Models;

public sealed record AccessTokenResult
{
    public AccessTokenResultState State { get; }

    public string? Text { get; }

    private AccessTokenResult(
        AccessTokenResultState state,
        string? text)
    {
        State = state;
        Text = text;
    }

    public static AccessTokenResult None() =>
       new AccessTokenResult(
           AccessTokenResultState.None,
           null);

    public static AccessTokenResult Succeeded(string text) =>
        new AccessTokenResult(
            AccessTokenResultState.Succeeded,
            text);

    public static AccessTokenResult Failed(string text) =>
        new AccessTokenResult(
            AccessTokenResultState.Failed,
            text);

    public static AccessTokenResult Cancelled() =>
        new AccessTokenResult(
            AccessTokenResultState.Cancelled,
            null);
}
