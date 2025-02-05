using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Core.AzureAd.Models;
using ClientTokenProvider.Core.Shared.Abstractions;

namespace ClientTokenProvider.Business.Shared.Services.Abstractions;

public interface IClientTokenProviderFactory
{
    public IClientTokenProvider Create(
        ConfigurationKind configurationKind,
        IClientTokenProviderConfiguration clientTokenProviderConfiguration);
}
