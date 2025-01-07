namespace ClientTokenProvider.Business.Shared.Models;

public record Configuration
{
    public required ConfigurationIdentity Identity { get; init; }

    public required ConfigurationKind Kind { get; init; }

    public required IConfigurationData Data { get; init; }
}
