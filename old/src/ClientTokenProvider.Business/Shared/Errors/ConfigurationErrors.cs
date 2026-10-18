namespace ClientTokenProvider.Business.Shared.Errors;

public static class ConfigurationErrors
{
    public static class Configuration
    {
        public static Error NotFound() => new(
            $"{nameof(Configuration)}.{nameof(NotFound)}",
            "Configuration not found",
            ErrorType.Validation);
    }
}
