using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Shared.Services.Abstractions;

public interface IConfigurationExporter
{
    public Task Export(
        ConfigurationModel configuration,
        CancellationToken cancellationToken);
}
