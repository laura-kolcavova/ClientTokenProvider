using ClientTokenProvider.Core.AzureAd.Models;
using ClientTokenProvider.Core.AzureAd.Providers;

namespace ClientTokenProvider.Core.AzureAd.Factories;

public interface IAzureAdClientTokenProviderFactory
{
    public IAzureAdClientTokenProvider Create(
        ClientConfiguration clientConfiguration);
}
