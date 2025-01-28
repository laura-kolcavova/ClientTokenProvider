using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Shared.Models;

public sealed record RenameConfigurationRequest
{
    public required ConfigurationModel Configuration { get; init; }

    public required string NewName { get; init; }
}
