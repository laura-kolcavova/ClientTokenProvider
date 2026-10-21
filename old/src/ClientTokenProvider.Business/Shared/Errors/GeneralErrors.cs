namespace ClientTokenProvider.Business.Shared.Errors;

public static class GeneralErrors
{
    public static class General
    {
        public static Error Unexpected() => new(
            $"{nameof(General)}.{nameof(Unexpected)}",
            "An unexpected error occurred",
            ErrorType.Unexpected);

        public static Error Cancelled() => new(
            $"{nameof(General)}.{nameof(Cancelled)}",
            "The operation was cancelled",
            ErrorType.Cancelled);
    }
}
