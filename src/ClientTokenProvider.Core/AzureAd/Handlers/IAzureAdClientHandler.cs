using ClientTokenProvider.Core.AzureAd.Dto;

namespace ClientTokenProvider.Core.AzureAd.Handlers;

public interface IAzureAdClientHandler
{
    public Task<GetClientTokenResponse> GetClientToken(
        string scope,
        CancellationToken cancellationToken);
}
