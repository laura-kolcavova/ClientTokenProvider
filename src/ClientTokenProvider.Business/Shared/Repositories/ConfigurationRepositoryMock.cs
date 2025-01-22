using ClientTokenProvider.Business.AzureAd.Models;
using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Business.Shared.Repositories;

internal sealed class ConfigurationRepositoryMock :
    IConfigurationRepository
{
    private List<Configuration> _configurations = new List<Configuration>()
        {
            new Configuration
            {
                Id = Guid.NewGuid(),
                Name = "Configuration 1",
                Kind = ConfigurationKind.AzureAd,
                Data = AzureAdConfigurationData.Empty,
            },

            new Configuration
            {
                Id = Guid.NewGuid(),
                Name = "Configuration 2",
                Kind = ConfigurationKind.AzureAd,
                Data = AzureAdConfigurationData.Empty,
            }
        };

    public async Task<Configuration?> Get(
        Guid configurationId,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        return _configurations
            .FirstOrDefault(configuration => configuration.Id == configurationId);
    }

    public async Task<IReadOnlyCollection<Configuration>> GetAll(
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        return _configurations.ToList();
    }
}
