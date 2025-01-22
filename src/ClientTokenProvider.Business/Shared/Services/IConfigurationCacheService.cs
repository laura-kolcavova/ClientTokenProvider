using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Business.Shared.Services;

public interface IConfigurationCacheService
{
    public void Add(Configuration configuration);

    public void AddMany(IEnumerable<Configuration> configurations);

    public Configuration? Get(Guid configurationId);

    public IReadOnlyCollection<Configuration>? GetAll();

    public void Update(Configuration configuration);

    public void Remove(Guid configurationId);
}
