namespace ClientTokenProvider.Business.Shared.Models;

public class ConfigurationModel
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required ConfigurationKind Kind { get; init; }

    public required IConfigurationData Data { get; init; }
}
