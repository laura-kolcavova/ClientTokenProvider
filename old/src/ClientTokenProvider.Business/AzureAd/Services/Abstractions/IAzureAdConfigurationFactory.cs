using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Business.AzureAd.Services.Abstractions;

public interface IAzureAdConfigurationFactory
{
    public ConfigurationModel Create();
}