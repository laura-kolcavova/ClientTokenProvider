using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Shared.Messages;

public sealed record ConfigurationSelectedMessage
{
    public required ConfigurationIdentity ConfigurationIdentity { get; init; }
}
