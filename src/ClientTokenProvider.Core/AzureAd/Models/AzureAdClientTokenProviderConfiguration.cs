namespace ClientTokenProvider.Core.AzureAd.Models;

public sealed class AzureAdClientTokenProviderConfiguration :
    IAzureAdClientTokenProviderConfiguration
{
    public string Instance { get; set; } = string.Empty;

    public string TenantId { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    public string ClientId { get; set; } = string.Empty;

    public string? ClientSecret { get; set; } = null;
}
