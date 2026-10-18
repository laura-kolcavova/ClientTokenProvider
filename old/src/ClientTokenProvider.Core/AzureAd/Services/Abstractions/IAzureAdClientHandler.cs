using ClientTokenProvider.Core.AzureAd.Dto;

namespace ClientTokenProvider.Core.AzureAd.Services.Abstractions;

public interface IAzureAdClientHandler
{
    public Task<GetClientTokenResponse> GetClientToken(
        string scope,
        CancellationToken cancellationToken);
}
