namespace KittyToken.Application.AzureAd.Models;

public sealed record AzureAdConfiguration
{
    public required string Instance { get; init; }

    public required string TenantId { get; init; }

    public required string ClientId { get; init; }

    public required string ClientSecret { get; init; }

    public required string Scope { get; init; }

    public static AzureAdConfiguration Empty()
    {
        return new AzureAdConfiguration
        {
            Instance = string.Empty,
            TenantId = string.Empty,
            ClientId = string.Empty,
            ClientSecret = string.Empty,
            Scope = string.Empty
        };
    }
}
