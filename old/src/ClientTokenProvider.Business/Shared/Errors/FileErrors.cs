namespace ClientTokenProvider.Business.Shared.Errors;

public static class FileErrors
{
    public static class File
    {
        public static Error SavingFailed() => new(
            $"{nameof(File)}.{nameof(SavingFailed)}",
            "Saving file failed",
            ErrorType.Failure);

        public static Error InvalidExtension() => new(
          $"{nameof(File)}.{nameof(InvalidExtension)}",
          "File extension is invalid",
          ErrorType.Failure);

        public static Error InvalidFormat() => new(
            $"{nameof(File)}.{nameof(InvalidFormat)}",
            "File format is invalid",
            ErrorType.Failure);
    }
}
