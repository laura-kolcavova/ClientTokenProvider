namespace ClientTokenProvider.Business.Shared.Exceptions;

public sealed class JsonDeserializationException : Exception
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
