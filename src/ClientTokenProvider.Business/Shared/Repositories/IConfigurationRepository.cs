using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Business.Shared.Repositories;

public interface IConfigurationRepository
{
    public Task<Configuration?> Get(
        Guid configurationId,
        CancellationToken cancellationToken);

    public Task<IReadOnlyCollection<Configuration>> GetAll(
        CancellationToken cancellationToken);
}
