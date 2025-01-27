using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Business.Shared.Services;

public interface IConfigurationActionStateStore
{
    public ConfigurationActionState Get(
        ConfigurationModel configuration);

    public void Set(
        ConfigurationModel configuration,
        ConfigurationActionState configurationActionState);

    public void Remove(
       ConfigurationModel configuration);
}
