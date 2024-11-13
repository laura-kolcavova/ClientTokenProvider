using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Business.Shared.Services;

public interface IConfigurationCache
{
    public void Save(ConfigurationBase configuration);

    public IReadOnlyCollection<ConfigurationBase>? GetAll();

    public Configuration<TConfigurationDataType>? Get<TConfigurationDataType>(
        Guid configurationId)
        where TConfigurationDataType : notnull;

    public void Remove(Guid configurationId);
}
