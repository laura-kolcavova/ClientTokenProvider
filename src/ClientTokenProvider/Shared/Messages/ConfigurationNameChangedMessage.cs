using ClientTokenProvider.Shared.Models;

namespace ClientTokenProvider.Shared.Messages;


internal sealed record ConfigurationNameChangedMessage
{
    public required ConfigurationIdentityModel ConfigurationIdentity { get; init; }

    public required bool IsConfigurationSaved { get; init; }
}
