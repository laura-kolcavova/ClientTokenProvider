namespace ClientTokenProvider.Core.AzureAd.Models;

public interface IClientConfiguration
{
    public string AuthorityUri { get; }

    public string Audience { get; }

    public string ClientId { get; }

    public string? ClientSecret { get; }
}
