using ClientTokenProvider.Application.AzureAd.Dto;

namespace ClientTokenProvider.Application.AzureAd.Interfaces;

public interface IAzureAdClientHandler
{
    public Task<GetClientTokenResponse> GetClientToken(
        string scope,
        CancellationToken cancellationToken);
}
