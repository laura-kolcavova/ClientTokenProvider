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
       new(
           AccessTokenResultState.None,
           null);

    public static AccessTokenResult Succeeded(string text) =>
        new(
            AccessTokenResultState.Succeeded,
            text);

    public static AccessTokenResult Failed(string text) =>
        new(
            AccessTokenResultState.Failed,
            text);

    public static AccessTokenResult Cancelled() =>
        new(
            AccessTokenResultState.Cancelled,
            null);
}
