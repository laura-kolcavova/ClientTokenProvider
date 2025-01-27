using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Business.Shared.Services.Abstractions;

public interface IConfigurationService
{
    public Task<ConfigurationModel> Create(
        ConfigurationKind kind,
        CancellationToken cancellationToken);

    public Task Save(
        ConfigurationModel configuration,
        CancellationToken cancellationToken);

    public ValueTask<IReadOnlyCollection<ConfigurationModel>> GetAll(
        CancellationToken cancellationToken);

    public ValueTask<ConfigurationModel?> Get(
       Guid configurationId,
       CancellationToken cancellationToken);

    public Task Delete(
       Guid configurationId,
       CancellationToken cancellationToken);
}
