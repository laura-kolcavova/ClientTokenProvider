using ClientTokenProvider.Shared.Models;

namespace ClientTokenProvider.Shared.Messages;


public sealed record ConfigurationNameChangedMessage
{
    public required ConfigurationIdentityModel ConfigurationIdentity { get; init; }

    public required bool IsConfigurationSaved { get; init; }
}
