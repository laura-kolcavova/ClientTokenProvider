namespace ClientTokenProvider.Business.Shared.Errors;

public static class AccessTokenErrors
{
    public static class AccessToken
    {
        public static Error RequestFailed(string message) => new(
            $"{nameof(AccessToken)}.{nameof(RequestFailed)}",
            message,
            ErrorType.Failure);
    }
}
