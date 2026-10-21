using ClientTokenProvider.Business.AzureAd.Models;
using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Business.Shared.Services.Abstractions;

namespace ClientTokenProvider.Business.AzureAd.Services;

internal sealed class ConfigurationDataTypeProvider :
    IConfigurationDataTypeProvider
{
    public Type Get(ConfigurationKind kind)
    {
        return typeof(AzureAdConfigurationData);
    }
}
