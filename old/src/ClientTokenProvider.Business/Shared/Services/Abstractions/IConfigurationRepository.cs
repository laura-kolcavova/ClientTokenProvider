using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Business.Shared.Services.Abstractions;

public interface IConfigurationRepository
{
    public Task<ConfigurationModel?> Get(
        Guid configurationId,
        CancellationToken cancellationToken);

    public Task<IReadOnlyCollection<ConfigurationModel>> GetAll(
        CancellationToken cancellationToken);

    public Task Add(
        ConfigurationModel configuration,
        CancellationToken cancellationToken);

    public Task Update(
        ConfigurationModel configuration,
        CancellationToken cancellationToken);

    public Task UpdateMany(
       IEnumerable<ConfigurationModel> configurations,
       CancellationToken cancellationToken);

    public Task Delete(
       ConfigurationModel configuration,
       CancellationToken cancellationToken);
}
