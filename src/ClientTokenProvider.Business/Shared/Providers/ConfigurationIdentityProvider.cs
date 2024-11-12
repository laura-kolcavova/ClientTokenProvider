using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Business.Shared.Providers;

internal sealed class ConfigurationIdentityProvider
    : IConfigurationIdentityProvider
{
    private const string DefaultName = "New Configuration";

    public ConfigurationIdentity NewIdentity()
    {
        return new ConfigurationIdentity
        {
            Id = Guid.NewGuid(),
            Name = DefaultName,
        };
    }
}
