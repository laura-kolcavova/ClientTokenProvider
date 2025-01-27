using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Business.Shared.Services.Abstractions;

public interface IConfigurationFactory
{
    public ConfigurationModel Create(ConfigurationKind kind);
}
