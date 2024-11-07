using System.Text.Json.Serialization;

namespace ClientTokenProvider.Core.AzureAd.Dto;

public sealed record GetClientTokenRequestItem
{
    [JsonPropertyName("key")]
    public required string Key { get; init; }

    [JsonPropertyName("value")]
    public required string Value { get; init; }

    [JsonPropertyName("disabled")]
    public required bool Disabled { get; init; }
}
