namespace ClientTokenProvider.Core.AzureAd.Models;

public sealed class ClientConfiguration : IClientConfiguration
{
    public string AuthorityUri { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    public string ClientId { get; set; } = string.Empty;

    public string? ClientSecret { get; set; } = null;
}
