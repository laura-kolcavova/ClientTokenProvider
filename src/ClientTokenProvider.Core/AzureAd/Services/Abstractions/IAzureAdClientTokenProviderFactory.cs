using ClientTokenProvider.Core.AzureAd.Models;

namespace ClientTokenProvider.Core.AzureAd.Services.Abstractions;

public interface IAzureAdClientTokenProviderFactory
{
    public IAzureAdClientTokenProvider Create(
        AzureAdClientTokenProviderConfiguration clientConfiguration);
}
