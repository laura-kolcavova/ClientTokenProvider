using ClientTokenProvider.AzureAd.BindableViewModels;
using ClientTokenProvider.AzureAd.Extensions;
using ClientTokenProvider.Business.AzureAd.Models;
using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Business.Shared.Models.Abstractions;
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
        if (source is AzureAdConfigurationData azureAdSource)
        {
            return azureAdSource.ToBindableModel();
        }
        else
        {
            throw new ArgumentException("Invalid configuration data type.", nameof(source));
        }
    }

    public IConfigurationData ToModel(
        IConfigurationDataBindableModel source,
        ConfigurationKind sourceConfigurationKind)
    {
        return ((AzureAdConfigurationDataBindableModel)source).ToModel();
    }
}
