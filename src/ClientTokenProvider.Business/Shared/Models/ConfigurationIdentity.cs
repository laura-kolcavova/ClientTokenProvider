namespace ClientTokenProvider.Business.Shared.Models;

public sealed record ConfigurationIdentity
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }
}