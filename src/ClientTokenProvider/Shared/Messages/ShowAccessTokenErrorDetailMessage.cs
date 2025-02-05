namespace ClientTokenProvider.Shared.Messages;

public sealed record ShowAccessTokenErrorDetailMessage
{
    public required string ErrorMessage { get; init; }
}
