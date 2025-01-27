using ClientTokenProvider.Business.AzureAd.Factories;
using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Business.Shared.Factories;

internal class ConfigurationFactory(
    IAzureAdConfigurationFactory azureAdConfigurationFactory) :
    IConfigurationFactory
{
    public ConfigurationModel Create(ConfigurationKind kind)
    {
        // Keyed services should be used instead
        if (kind == ConfigurationKind.AzureAd)
        {
            return azureAdConfigurationFactory.Create();
        }

        throw new ArgumentException($"Unsupported configuration kind {kind}");
    }
}
