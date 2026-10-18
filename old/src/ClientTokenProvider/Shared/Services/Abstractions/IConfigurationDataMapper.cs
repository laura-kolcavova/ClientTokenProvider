using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Business.Shared.Models.Abstractions;
using ClientTokenProvider.Shared.BindableModels.Abstractions;

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
