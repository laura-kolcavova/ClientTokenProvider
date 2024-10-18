namespace ClientTokenProvider.Application.AzureAd.Exceptions;

public class ClientHandlerException : Exception
{
    public int StatusCode { get; private set; }

    public string? Response { get; private set; }

    public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; private set; }

    public ClientHandlerException(
        string message,
        int statusCode,
        string? response,
        IReadOnlyDictionary<string, IEnumerable<string>> headers,
        Exception? innerException)
        : base(
            BuildMessage(message, statusCode, response),
            innerException)
    {
        StatusCode = statusCode;
        Response = response;
        Headers = headers;
    }

    public override string ToString()
    {
        return BuildMessage(Message, StatusCode, Response);
    }

    private static string BuildMessage(
        string message,
        int statusCode,
        string? response)
    {
        var responseString = response is null
            ? "(null)"
            : response[..((response.Length >= 512) ? 512 : response.Length)];

        return $"{message}\n\nStatus: {statusCode}\nResponse: \n{responseString}";
    }
}
