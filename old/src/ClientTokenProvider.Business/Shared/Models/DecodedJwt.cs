namespace ClientTokenProvider.Business.Shared.Models;

public sealed record DecodedJwt
{
    public string Header { get; init; } = string.Empty;

    public string Payload { get; init; } = string.Empty;

    public string Signature { get; init; } = string.Empty;
}
