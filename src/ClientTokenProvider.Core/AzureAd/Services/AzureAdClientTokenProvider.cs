using ClientTokenProvider.Core.AzureAd.Models;
using ClientTokenProvider.Core.AzureAd.Services.Abstractions;

namespace ClientTokenProvider.Core.AzureAd.Services;

public sealed class AzureAdClientTokenProvider : IAzureAdClientTokenProvider
{
    private readonly IAzureAdClientHandler _handler;

    public AzureAdClientTokenProvider(
        IHttpClientFactory httpClientFactory,
        ClientTokenProviderConfiguration clientTokenProviderConfiguration)
    {
        _handler = new AzureAdClientHandler(
            httpClientFactory,
            clientTokenProviderConfiguration);
    }

    public async Task<string> GetAccessToken(string scope, CancellationToken cancellationToken)
    {
        var clientToken = await _handler.GetClientToken(scope, cancellationToken);

        return clientToken.AccessToken;
    }
}
