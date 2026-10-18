using KittyToken.Application.AzureAd.Dto;
using KittyToken.Application.AzureAd.Services.Abstractions;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

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

        var clientId = request.ClientId;
        var clientSecret = request.ClientSecret;
        var scope = request.Scope;

        var requestBody = new FormUrlEncodedContent(new[]
        {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("scope", scope),
            });

        var response = await httpClient.PostAsync(
            requestUri,
            requestBody,
            cancellationToken);

        try
        {
            var status = (int)response.StatusCode;

            var headers = response.Headers.ToDictionary(
                keySelector => keySelector.Key,
                valueSelector => valueSelector.Value);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseObject = await ReadAsJson<GetClientTokenResponseDto>(
                    response,
                    headers,
                    cancellationToken)
                    ?? throw new ClientHandlerException(
                        "Response is null.",
                        status,
                        string.Empty,
                        headers,
                        null);

                return responseObject;
            }
            else
            {
                var text = response.Content is not null
                    ? await response.Content.ReadAsStringAsync(cancellationToken)
                    : null;

                throw new ClientHandlerException(
                    $"The HTTP status code of the response was not expected {status}",
                    status,
                    text,
                    headers,
                    null);
            }
        }
        finally
        {
            response.Dispose();
        }
    }

    private static async Task<TResponseObject?> ReadAsJson<TResponseObject>(
        HttpResponseMessage response,
        IReadOnlyDictionary<string, IEnumerable<string>> headers,
        CancellationToken cancellationToken)
    {
        if (response is null || response.Content is null)
        {
            return default;
        }

        try
        {
            var responseObject = await response
                .Content
                .ReadFromJsonAsync<TResponseObject>(cancellationToken);

            return responseObject;
        }
        catch (JsonException ex)
        {
            var text = await response.Content.ReadAsStringAsync(cancellationToken);

            throw new ClientHandlerException(
                $"Could not deserialize the response body string as {typeof(TResponseObject).FullName}.",
                (int)response.StatusCode,
                text,
                headers,
                ex);
        }
    }
}

