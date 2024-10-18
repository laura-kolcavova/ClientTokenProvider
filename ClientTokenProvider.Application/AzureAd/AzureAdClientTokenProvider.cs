
using ClientTokenProvider.Application.AzureAd.Interfaces;

namespace ClientTokenProvider.Application.AzureAd;

internal sealed class AzureAdClientTokenProvider : IClientTokenProvider
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
