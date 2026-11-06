namespace KittyToken.Application.AzureAd.Dto;

public sealed record GetClientTokenRequestDto
{
    public required string Instance { get; init; }

    public required string TenantId { get; init; }

    public required string ClientId { get; init; }

    public required string ClientSecret { get; init; }

    public required string Scope { get; init; }
}
