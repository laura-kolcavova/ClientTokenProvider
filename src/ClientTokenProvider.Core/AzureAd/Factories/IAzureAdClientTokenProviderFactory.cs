using ClientTokenProvider.Core.AzureAd.Models;

namespace ClientTokenProvider.Core.AzureAd.Factories;

public interface IAzureAdClientTokenProviderFactory
{
    public IAzureAdClientTokenProvider Create(
        ClientConfiguration clientConfiguration);
}
