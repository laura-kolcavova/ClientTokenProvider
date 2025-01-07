using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Business.AzureAd.Factories;

public interface IAzureAdConfigurationFactory
{
    public Configuration Create();
}