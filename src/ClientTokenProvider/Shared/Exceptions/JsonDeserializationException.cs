namespace ClientTokenProvider.Shared.Exceptions;

internal sealed class JsonDeserializationException : Exception
{
    public JsonDeserializationException(string message)
        : base(message)
    {
    }

    public JsonDeserializationException(
        string message,
        Exception? innerException)
        : base(message, innerException)
    {
    }
}
