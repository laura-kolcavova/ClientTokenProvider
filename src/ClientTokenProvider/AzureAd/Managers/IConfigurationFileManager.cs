using ClientTokenProvider.AzureAd.Models;
using CSharpFunctionalExtensions;

namespace ClientTokenProvider.AzureAd.Managers;

public interface IConfigurationFileManager
{
    public Task<UnitResult<string>> SaveConfiguration(
       string configurationName,
       ClientConfigurationModel configuration,
       CancellationToken cancellationToken);

    public Task<Result<ClientConfigurationModel, string>> OpenConfiguration(
      string configurationName,
      CancellationToken cancellationToken);
}
