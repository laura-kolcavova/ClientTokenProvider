using ClientTokenProvider.Business.AzureAd.Services.Abstractions;
using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Business.Shared.Services.Abstractions;

namespace ClientTokenProvider.Business.Shared.Services;

internal sealed class ConfigurationFactory(
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
