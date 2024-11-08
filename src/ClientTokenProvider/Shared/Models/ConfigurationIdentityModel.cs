namespace ClientTokenProvider.Shared.Models;

public sealed record ConfigurationIdentityModel
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }
}