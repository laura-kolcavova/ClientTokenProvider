using ClientTokenProvider.Shared.Models;

namespace ClientTokenProvider.Shared.Messages;

internal sealed record ConfigurationSelectedMessage
{
    public required ConfigurationIdentityModel ConfigurationIdentity { get; init; }
}
