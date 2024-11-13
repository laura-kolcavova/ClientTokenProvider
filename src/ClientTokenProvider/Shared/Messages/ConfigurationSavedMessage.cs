using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Shared.Messages;

public sealed record ConfigurationSavedMessage
{
    public required ConfigurationIdentity ConfigurationIdentity { get; init; }
}
