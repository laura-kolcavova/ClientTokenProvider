namespace ClientTokenProvider.Core.AzureAd.Dto;

public sealed record GetClientTokenResponse
{
    public required string AccessToken { get; init; }
}
