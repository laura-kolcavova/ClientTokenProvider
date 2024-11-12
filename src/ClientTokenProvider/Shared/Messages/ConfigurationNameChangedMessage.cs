using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Shared.Messages;

public sealed record ConfigurationNameChangedMessage
{
    public required ConfigurationIdentity ConfigurationIdentity { get; init; }

    public required bool IsConfigurationSaved { get; init; }
}
