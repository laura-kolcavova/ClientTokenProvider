using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Business.Shared.Services;

public interface IConfigurationService
{
    public Task<Configuration> Create(
        ConfigurationKind kind,
        CancellationToken cancellationToken);

    public Task Save(
        Configuration configuration,
        CancellationToken cancellationToken);

    public ValueTask<IReadOnlyCollection<Configuration>> GetAll(
        CancellationToken cancellationToken);

    public ValueTask<Configuration?> Get(
       Guid configurationId,
       CancellationToken cancellationToken);
}
