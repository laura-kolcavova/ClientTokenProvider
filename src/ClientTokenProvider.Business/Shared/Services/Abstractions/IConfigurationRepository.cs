using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Business.Shared.Services.Abstractions;

public interface IConfigurationRepository
{
    public Task<ConfigurationModel?> Get(
        Guid configurationId,
        CancellationToken cancellationToken);

    public Task<IReadOnlyCollection<ConfigurationModel>> GetAll(
        CancellationToken cancellationToken);
}
