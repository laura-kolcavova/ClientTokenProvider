namespace ClientTokenProvider.Application.AzureAd.Interfaces;

public interface IClientConfiguration
{
    public string AuthorityUri { get; }

    public string Scope { get; }

    public string Audience { get; }

    public string ClientId { get; }

    public string? ClientSecret { get; }
}
