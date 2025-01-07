using ClientTokenProvider.Business.AzureAd.Models;
using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Business.Shared.Providers;

namespace ClientTokenProvider.Business.AzureAd.Factories;

internal sealed class AzureAdConfigurationFactory
    (IConfigurationIdentityProvider configurationIdentityProvider) :
    IAzureAdConfigurationFactory
{
    public Configuration Create()
    {
        var identity = configurationIdentityProvider.NewIdentity();

        var data = AzureAdConfigurationData.Empty;

        var configuration = new Configuration()
        {
            Kind = ConfigurationKind.AzureAd,
            Identity = identity,
            Data = data
        };

        return configuration;
    }
}
