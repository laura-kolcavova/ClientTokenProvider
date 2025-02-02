using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Shared.Services.Abstractions;

public interface IConfigurationDataMapper
{
    public IConfigurationDataBindableModel ToBindableModel(
        IConfigurationData source,
        ConfigurationKind sourceConfigurationKind);

    public IConfigurationData ToModel(
        IConfigurationDataBindableModel source,
        ConfigurationKind sourceConfigurationKind);
}
