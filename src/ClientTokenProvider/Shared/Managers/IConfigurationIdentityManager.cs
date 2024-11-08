using ClientTokenProvider.Shared.Models;

namespace ClientTokenProvider.Shared.Managers;

public interface IConfigurationIdentityManager
{
    public ConfigurationIdentityModel NewIdentity();
}