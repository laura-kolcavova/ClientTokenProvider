namespace ClientTokenProvider.AzureAd.Models;

public sealed record ClientConfigurationModel
{
    public required string AuthorityUrl { get; init; }

    public required string Scope { get; init; }

    public required string Audience { get; init; }

    public required string ClientId { get; init; }

    public required string ClientSecret { get; init; }

    public static ClientConfigurationModel Empty =>
        new()
        {
            AuthorityUrl = string.Empty,
            Scope = string.Empty,
            Audience = string.Empty,
            ClientId = string.Empty,
            ClientSecret = string.Empty,
        };
}