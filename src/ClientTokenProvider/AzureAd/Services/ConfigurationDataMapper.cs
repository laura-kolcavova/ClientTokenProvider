using ClientTokenProvider.AzureAd.BindableViewModels;
using ClientTokenProvider.AzureAd.Extensions;
using ClientTokenProvider.Business.AzureAd.Models;
using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Shared.BindableModels.Abstractions;
using ClientTokenProvider.Shared.Services.Abstractions;

namespace ClientTokenProvider.AzureAd.Services;

internal sealed class ConfigurationDataMapper :
    IConfigurationDataMapper
{
    public IConfigurationDataBindableModel ToBindableModel(
        IConfigurationData source,
        ConfigurationKind sourceConfigurationKind)
    {
        return ((AzureAdConfigurationData)source).ToBindableModel();
    }

    public IConfigurationData ToModel(
        IConfigurationDataBindableModel source,
        ConfigurationKind sourceConfigurationKind)
    {
        return ((ConfigurationDataBindableModel)source).ToModel();
    }
}
