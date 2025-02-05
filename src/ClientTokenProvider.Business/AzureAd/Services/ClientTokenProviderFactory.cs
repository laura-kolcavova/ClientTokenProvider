using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Business.Shared.Services.Abstractions;
using ClientTokenProvider.Core.AzureAd.Models;
using ClientTokenProvider.Core.AzureAd.Services.Abstractions;
using ClientTokenProvider.Core.Shared.Abstractions;

namespace ClientTokenProvider.Business.AzureAd.Services;

internal sealed class ClientTokenProviderFactory(
    IAzureAdClientTokenProviderFactory azureAdClientTokenProviderFactory) :
    IClientTokenProviderFactory
{
    public IClientTokenProvider Create(
        ConfigurationKind configurationKind,
        IClientTokenProviderConfiguration clientTokenProviderConfiguration)
    {
        if (clientTokenProviderConfiguration is not ClientTokenProviderConfiguration azureAdClientTokenProviderConfiguration)
        {
            throw new ArgumentException("Invalid configuration data type.", nameof(clientTokenProviderConfiguration));
        }

        return azureAdClientTokenProviderFactory.Create(
            azureAdClientTokenProviderConfiguration);
    }
}
