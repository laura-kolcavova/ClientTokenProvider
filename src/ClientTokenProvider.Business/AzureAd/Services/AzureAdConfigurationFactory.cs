using ClientTokenProvider.Business.AzureAd.Models;
using ClientTokenProvider.Business.AzureAd.Services.Abstractions;
using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Business.AzureAd.Services;

internal sealed class AzureAdConfigurationFactory :
    IAzureAdConfigurationFactory
{
    public ConfigurationModel Create()
    {
        var data = AzureAdConfigurationData.Empty;

        var configuration = new ConfigurationModel(
            id: Guid.NewGuid(),
            kind: ConfigurationKind.AzureAd,
            data: data);

        return configuration;
    }
}
