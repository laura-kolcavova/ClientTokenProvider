namespace ClientTokenProvider.Core.AzureAd.Dto;

public sealed record GetClientTokenRequestItem
{
    public required string Key { get; init; }

    public required string Value { get; init; }

    public required bool Disabled { get; init; }
}
