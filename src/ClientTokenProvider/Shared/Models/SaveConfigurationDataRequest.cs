using ClientTokenProvider.Business.Shared.Models.Abstractions;

namespace ClientTokenProvider.Shared.Models;

public sealed record SaveConfigurationDataRequest
{
    public required Guid ConfigurationId { get; init; }

    public required IConfigurationData ConfigurationData { get; init; }
}
