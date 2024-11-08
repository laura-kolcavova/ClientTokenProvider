namespace ClientTokenProvider.Shared.Models;

public sealed record ConfigurationListItemModel
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }
}
