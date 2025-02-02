using ClientTokenProvider.AzureAd.BindableViewModels;
using ClientTokenProvider.Business.AzureAd.Models;

namespace ClientTokenProvider.AzureAd.Extensions;

public static class ConfigurationDataExtensions
{
    public static ConfigurationDataBindableModel ToBindableModel(
        this AzureAdConfigurationData source)
    {
        return new ConfigurationDataBindableModel(
            authorityUrl: source.AuthorityUrl,
            scope: source.Scope,
            audience: source.Audience,
            clientId: source.ClientId,
            clientSecret: source.ClientSecret);
    }

    public static AzureAdConfigurationData ToModel(
        this ConfigurationDataBindableModel source)
    {
        return new AzureAdConfigurationData
        {
            AuthorityUrl = source.AuthorityUrl,
            Scope = source.Scope,
            Audience = source.Audience,
            ClientId = source.ClientId,
            ClientSecret = source.ClientSecret
        };
    }
}
