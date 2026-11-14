using KittyToken.Application.AzureAd.Dto;

namespace KittyToken.Application.AzureAd.Services.Abstractions;

public interface IAzureAdClient
{
    public Task<string> GetAccessToken(
        GetClientTokenRequestDto request,
        CancellationToken cancellationToken);
}
