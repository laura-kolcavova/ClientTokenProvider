using ClientTokenProvider.Business.AzureAd.Models;
using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Business.Shared.Repositories;

internal sealed class ConfigurationRepositoryMock :
    IConfigurationRepository
{
    private List<ConfigurationModel> _configurations = new List<ConfigurationModel>()
        {
            new ConfigurationModel
            {
                Id = Guid.NewGuid(),
                Name = "Configuration 1",
                Kind = ConfigurationKind.AzureAd,
                Data = AzureAdConfigurationData.Empty,
            },

            new ConfigurationModel
            {
                Id = Guid.NewGuid(),
                Name = "Configuration 2",
                Kind = ConfigurationKind.AzureAd,
                Data = AzureAdConfigurationData.Empty,
            }
        };

    public async Task<ConfigurationModel?> Get(
        Guid configurationId,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        return _configurations
            .FirstOrDefault(configuration => configuration.Id == configurationId);
    }

    public async Task<IReadOnlyCollection<ConfigurationModel>> GetAll(
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        return _configurations.ToList();
    }
}
