using System.Text.Json.Serialization;

namespace ClientTokenProvider.Core.AzureAd.Dto;

public sealed record GetClientTokenResponse
{
    [JsonPropertyName("token_type")]
    public required string TokenType { get; init; }

    [JsonPropertyName("expires_in")]
    public required int ExpiresIn { get; init; }

    [JsonPropertyName("ext_expires_in")]
    public required int ExtExpiresIn { get; init; }

    [JsonPropertyName("access_token")]
    public required string AccessToken { get; init; }
}
