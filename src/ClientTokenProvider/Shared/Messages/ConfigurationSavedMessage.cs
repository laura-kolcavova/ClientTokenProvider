using ClientTokenProvider.Shared.Models;

namespace ClientTokenProvider.Shared.Messages;

public sealed record ConfigurationSavedMessage
{
    public required ConfigurationIdentityModel ConfigurationIdentity { get; init; }
}
