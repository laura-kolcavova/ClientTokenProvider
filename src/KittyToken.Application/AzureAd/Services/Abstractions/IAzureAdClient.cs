using KittyToken.Application.AzureAd.Dto;

namespace KittyToken.Application.AzureAd.Services.Abstractions;

public interface IAzureAdClient
{
    public Task<GetClientTokenResponseDto> GetClientToken(
        GetClientTokenRequestDto request,
        CancellationToken cancellationToken);
}
