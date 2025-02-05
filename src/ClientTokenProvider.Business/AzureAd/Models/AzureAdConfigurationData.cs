using ClientTokenProvider.Business.AzureAd.Mappers;
using ClientTokenProvider.Business.Shared.Models.Abstractions;
using ClientTokenProvider.Core.AzureAd.Models;

namespace ClientTokenProvider.Business.AzureAd.Models;

public sealed record AzureAdConfigurationData :
    IConfigurationData
{
    public required string AuthorityUrl { get; init; }

    public required string Scope { get; init; }

    public required string Audience { get; init; }

    public required string ClientId { get; init; }

    public required string ClientSecret { get; init; }

    public static AzureAdConfigurationData Empty =>
        new()
        {
            AuthorityUrl = string.Empty,
            Scope = string.Empty,
            Audience = string.Empty,
            ClientId = string.Empty,
            ClientSecret = string.Empty,
        };

    public IClientTokenProviderConfiguration ToClientTokenProviderConfiguration()
    {
        return AzureAdConfigurationDataMapper.ToClientConfiguration(this);
    }
}