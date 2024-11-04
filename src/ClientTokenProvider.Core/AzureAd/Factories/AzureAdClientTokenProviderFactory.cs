using ClientTokenProvider.Core.AzureAd.Models;
using ClientTokenProvider.Core.AzureAd.Providers;

namespace ClientTokenProvider.Core.AzureAd.Factories;

internal sealed class AzureAdClientTokenProviderFactory(
    IHttpClientFactory httpClientFactory) : IAzureAdClientTokenProviderFactory
{
    public IAzureAdClientTokenProvider Create(ClientConfiguration clientConfiguration)
    {
        return new AzureAdClientTokenProvider(httpClientFactory, clientConfiguration);
    }
}
