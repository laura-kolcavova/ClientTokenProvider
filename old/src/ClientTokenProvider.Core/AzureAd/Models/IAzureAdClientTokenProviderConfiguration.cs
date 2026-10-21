namespace ClientTokenProvider.Core.AzureAd.Models;

public interface IAzureAdClientTokenProviderConfiguration
{
    public string Instance { get; }

    public string TenantId { get; }

    public string Audience { get; }

    public string ClientId { get; }

    public string? ClientSecret { get; }
}
