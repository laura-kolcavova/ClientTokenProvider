namespace ClientTokenProvider.Application.AzureAd.Dto;

public sealed record GetClientTokenRequest
{
    public required string Mode { get; init; }

    public required IReadOnlyCollection<GetClientTokenRequestItem> UrlEncoded { get; init; }

    public static GetClientTokenRequest Create(
        string scope,
        string audience,
        string clientId,
        string? clientSecret)
    {
        var urlEncoded = new List<GetClientTokenRequestItem>()
        {
            new()
            {
                Key = "grant_type",
                Value = "client_credentials",
                Disabled = false,
            },
            new() {
                Key = "scope",
                Value = scope,
                Disabled = false,
            },
            new() {
                Key = "audience",
                Value = audience,
                Disabled = false,
            },
            new() {
                Key = "client_id",
                Value = clientId,
                Disabled = false,
            }
        };

        if (clientSecret is not null)
        {
            urlEncoded.Add(new GetClientTokenRequestItem
            {
                Key = "client_secret",
                Value = clientSecret,
                Disabled = false,
            });
        }

        return new GetClientTokenRequest
        {
            Mode = "urlencoded",
            UrlEncoded = urlEncoded
        };
    }
}
