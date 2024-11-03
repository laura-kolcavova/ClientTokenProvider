namespace ClientTokenProvider.AzureAd.Messages;

internal sealed record ShowErrorDetailMessage
{
    public required string ErrorMessage { get; init; }
}
