using KittyToken.Application.AzureAd.Dto;

namespace KittyToken.Application.AzureAd.Services.Abstractions;

public interface IAzureAdClient
{
    public Task<string> GetAcessToken(
        GetClientTokenRequestDto request,
        CancellationToken cancellationToken);
}
