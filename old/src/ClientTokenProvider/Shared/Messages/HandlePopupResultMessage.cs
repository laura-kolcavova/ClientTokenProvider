namespace ClientTokenProvider.Shared.Messages;

public sealed record HandlePopupResultMessage<TPopupResult>
{
    public required TPopupResult Result { get; init; }
}
