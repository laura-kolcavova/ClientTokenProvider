using ClientTokenProvider.AzureAd.Models;
using ClientTokenProvider.Shared.Models;
using CSharpFunctionalExtensions;

namespace ClientTokenProvider.AzureAd.Managers;

public interface IAzureAdConfigurationManager
{
    public Task<UnitResult<string>> SaveConfiguration(
       ConfigurationIdentityModel configurationIdentity,
       ClientConfigurationModel configuration,
       CancellationToken cancellationToken);

    public Task<Result<ClientConfigurationModel, string>> OpenConfiguration(
      ConfigurationIdentityModel configurationIdentity,
      CancellationToken cancellationToken);
}
