using ClientTokenProvider.Application.AzureAd.Dto;
using ClientTokenProvider.Application.AzureAd.Exceptions;
using ClientTokenProvider.Application.AzureAd.Interfaces;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace ClientTokenProvider.Application.AzureAd;

public sealed class AzureAdClientHandler : IAzureAdClientHandler
{
    private readonly IHttpClientFactory _httpClientFactory;

    public IClientConfiguration Configuration { get; }

    public AzureAdClientHandler(
        IHttpClientFactory httpClientFactory,
        ClientConfiguration clientConfiguration)
    {
        _httpClientFactory = httpClientFactory;
        Configuration = clientConfiguration;
    }

    public async Task<GetClientTokenResponse> GetClientToken(
        string scope,
        CancellationToken cancellationToken)
    {
        using (var httpClient = _httpClientFactory.CreateClient())
        {
            using (var request = new HttpRequestMessage())
            {
                request.Method = new HttpMethod("POST");
                request.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                request.RequestUri = new Uri(Configuration.AuthorityUri, UriKind.Absolute);

                var bodyRequest = GetClientTokenRequest.Create(
                    Configuration.Scope,
                    Configuration.Audience,
                    Configuration.ClientId,
                    Configuration.ClientSecret);

                var bodyRequestJson = JsonSerializer.Serialize(bodyRequest);

                request.Content = new StringContent(bodyRequestJson);

                var response = await httpClient.SendAsync(
                    request,
                    HttpCompletionOption.ResponseHeadersRead,
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
                            ? (await response.Content.ReadAsStringAsync(cancellationToken))
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
