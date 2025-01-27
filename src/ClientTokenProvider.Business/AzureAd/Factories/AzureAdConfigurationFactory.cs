using ClientTokenProvider.Business.AzureAd.Models;
using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Business.AzureAd.Factories;

internal sealed class AzureAdConfigurationFactory :
    IAzureAdConfigurationFactory
{
    public ConfigurationModel Create()
    {
        var data = AzureAdConfigurationData.Empty;

        var configuration = new ConfigurationModel()
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            Kind = ConfigurationKind.AzureAd,
            Data = data
        };

        return configuration;
    }
}
