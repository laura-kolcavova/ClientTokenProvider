namespace ClientTokenProvider.Application.AzureAd.Dto;

public sealed record GetClientTokenResponse
{
    public required string AccessToken { get; init; }
}
