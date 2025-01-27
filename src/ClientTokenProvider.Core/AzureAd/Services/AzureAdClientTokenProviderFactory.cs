using ClientTokenProvider.Core.AzureAd.Models;
using ClientTokenProvider.Core.AzureAd.Services.Abstractions;

namespace ClientTokenProvider.Core.AzureAd.Services;

internal sealed class AzureAdClientTokenProviderFactory(
    IHttpClientFactory httpClientFactory) : IAzureAdClientTokenProviderFactory
{
    public IAzureAdClientTokenProvider Create(ClientTokenProviderConfiguration clientConfiguration)
    {
        return new AzureAdClientTokenProvider(httpClientFactory, clientConfiguration);
    }
}
