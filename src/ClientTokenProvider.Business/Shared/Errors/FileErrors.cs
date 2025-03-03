namespace ClientTokenProvider.Business.Shared.Errors;

public static class FileErrors
{
    public static class File
    {
        public static Error SavingFailed() => new(
            $"{nameof(File)}.{nameof(SavingFailed)}",
            "Saving file failed",
            ErrorType.Failure);
    }
}
