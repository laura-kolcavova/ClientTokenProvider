using ClientTokenProvider.Shared.Models;

namespace ClientTokenProvider.Shared.Managers;

internal sealed class ConfigurationIdentityManager
    : IConfigurationIdentityManager
{
    private const string DefaultName = "New Configuration";

    public ConfigurationIdentityModel NewIdentity()
    {
        return new ConfigurationIdentityModel
        {
            Id = Guid.NewGuid(),
            Name = DefaultName,
        };
    }
}
