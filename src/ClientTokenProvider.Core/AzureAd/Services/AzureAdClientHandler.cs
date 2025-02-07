using ClientTokenProvider.Core.AzureAd.Dto;
using ClientTokenProvider.Core.AzureAd.Exceptions;
using ClientTokenProvider.Core.AzureAd.Models;
using ClientTokenProvider.Core.AzureAd.Services.Abstractions;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace ClientTokenProvider.Core.AzureAd.Services;

internal sealed class AzureAdClientHandler : IAzureAdClientHandler
{
    private readonly IHttpClientFactory _httpClientFactory;

    public IClientTokenProviderConfiguration Configuration { get; }

    public AzureAdClientHandler(
        IHttpClientFactory httpClientFactory,
        ClientTokenProviderConfiguration clientTokenProviderConfiguration)
    {
        _httpClientFactory = httpClientFactory;
        Configuration = clientTokenProviderConfiguration;
    }

    public async Task<GetClientTokenResponse> GetClientToken(
        string scope,
        CancellationToken cancellationToken)
    {
        using (var httpClient = _httpClientFactory.CreateClient())
        {
            var requestUri = new Uri(
                Configuration.AuthorityUri,
                UriKind.Absolute);

            var requestBody = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("scope", scope),
                new KeyValuePair<string, string>("audience", Configuration.Audience),
                new KeyValuePair<string, string>("client_id", Configuration.ClientId),
                new KeyValuePair<string, string>("client_secret", Configuration.ClientSecret ?? string.Empty),
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
                    var responseObject = await ReadAsJson<GetClientTokenResponse>(
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
