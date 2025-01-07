using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Business.Shared.Services;

public interface IConfigurationCache
{
    public void Save(Configuration configuration);

    public IReadOnlyCollection<Configuration>? GetAll();

    public Configuration? Get(
        Guid configurationId);

    public void Remove(Guid configurationId);
}
