using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Business.Shared.Factories;

public interface IConfigurationFactory
{
    public Configuration Create(ConfigurationKind kind);
}
