using ClientTokenProvider.Core.AzureAd.Handlers;
using ClientTokenProvider.Core.AzureAd.Models;

namespace ClientTokenProvider.Core.AzureAd;

public sealed class AzureAdClientTokenProvider : IAzureAdClientTokenProvider
{
    private readonly IAzureAdClientHandler _handler;

    public AzureAdClientTokenProvider(
        IHttpClientFactory httpClientFactory,
        ClientConfiguration clientConfiguration)
    {
        _handler = new AzureAdClientHandler(
            httpClientFactory,
            clientConfiguration);
    }

    public async Task<string> GetAccessToken(string scope, CancellationToken cancellationToken)
    {
        var clientToken = await _handler.GetClientToken(scope, cancellationToken);

        return clientToken.AccessToken;
    }
}
