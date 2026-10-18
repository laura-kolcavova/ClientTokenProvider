namespace KittyToken.Application.AzureAd.Exceptions;

public sealed class AzureAdClientException(
    int statusCode,
    string response)
    : Exception($"Azure AD token request failed ({statusCode}): {response}")
{
    public int StatusCode { get; } = statusCode;

    public string Response { get; } = response;
}
