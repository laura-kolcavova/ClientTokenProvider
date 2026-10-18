using ClientTokenProvider.AzureAd.BindableViewModels;
using ClientTokenProvider.Business.AzureAd.Models;

namespace ClientTokenProvider.AzureAd.Extensions;

public static class ConfigurationDataExtensions
{
    public static AzureAdConfigurationDataBindableModel ToBindableModel(
        this AzureAdConfigurationData source)
    {
        return new AzureAdConfigurationDataBindableModel(
            instance: source.Instance,
            tenantId: source.TenantId,
            scope: source.Scope,
            audience: source.Audience,
            clientId: source.ClientId,
            clientSecret: source.ClientSecret);
    }

    public static AzureAdConfigurationData ToModel(
        this AzureAdConfigurationDataBindableModel source)
    {
        return new AzureAdConfigurationData
        {
            Instance = source.Instance,
            TenantId = source.TenantId,
            Scope = source.Scope,
            Audience = source.Audience,
            ClientId = source.ClientId,
            ClientSecret = source.ClientSecret
        };
    }
}
