using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Business.Shared.Providers;

public interface IConfigurationIdentityProvider
{
    public ConfigurationIdentity NewIdentity();
}