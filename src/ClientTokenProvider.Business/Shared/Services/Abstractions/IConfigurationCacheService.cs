using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Business.Shared.Services.Abstractions;

public interface IConfigurationCacheService
{
    public void Add(ConfigurationModel configuration);

    public void AddMany(IEnumerable<ConfigurationModel> configurations);

    public ConfigurationModel? Get(Guid configurationId);

    public IReadOnlyCollection<ConfigurationModel>? GetAll();

    public void Update(ConfigurationModel configuration);

    public void Remove(Guid configurationId);
}
