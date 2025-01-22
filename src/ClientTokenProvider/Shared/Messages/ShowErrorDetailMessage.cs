namespace ClientTokenProvider.Shared.Messages;

internal sealed record ShowErrorDetailMessage
{
    public required string ErrorMessage { get; init; }
}
