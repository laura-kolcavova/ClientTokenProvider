using KittyToken.Application.AzureAd.Dto;
using KittyToken.Application.AzureAd.Exceptions;
using KittyToken.Application.AzureAd.Services.Abstractions;
using System.Net.Http.Json;

namespace KittyToken.Application.AzureAd.Services;

internal sealed class AzureAdClient(
    HttpClient httpClient) :
    IAzureAdClient
{
    public async Task<GetClientTokenResponseDto> GetClientToken(
        GetClientTokenRequestDto request,
        CancellationToken cancellationToken)
    {
        var instance = request.Instance.TrimEnd('/');
        var tenantId = request.TenantId.TrimEnd('/');

        var requestUri = new Uri(
            $"{instance}/{tenantId}/oauth2/v2.0/token",
            UriKind.Absolute);

        using var requestBody = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = request.ClientId,
            ["client_secret"] = request.ClientSecret,
            ["scope"] = request.Scope,
        });

        using var response = await httpClient.PostAsync(
            requestUri,
            requestBody,
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var text = await response.Content.ReadAsStringAsync(cancellationToken);

            throw new AzureAdClientException(
                (int)response.StatusCode,
                text);
        }

        var responseDto = await response.Content.ReadFromJsonAsync<GetClientTokenResponseDto>(cancellationToken)
            ?? throw new AzureAdClientException(
                (int)response.StatusCode,
                "Response is null.");

        return responseDto;
    }
}

