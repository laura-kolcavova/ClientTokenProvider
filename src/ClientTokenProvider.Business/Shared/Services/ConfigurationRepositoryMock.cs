using ClientTokenProvider.Business.AzureAd.Models;
using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Business.Shared.Services.Abstractions;

namespace ClientTokenProvider.Business.Shared.Services;

internal sealed class ConfigurationRepositoryMock :
    IConfigurationRepository
{
    private List<ConfigurationModel> _configurations = new List<ConfigurationModel>()
    {
        new ConfigurationModel(
            id: Guid.NewGuid(),
            name: "Configuration 1",
            kind: ConfigurationKind.AzureAd,
            data: AzureAdConfigurationData.Empty),

        new ConfigurationModel(
            id: Guid.NewGuid(),
            name: "Configuration 2",
            kind: ConfigurationKind.AzureAd,
            data: AzureAdConfigurationData.Empty),
    };

    public Task Add(ConfigurationModel configuration, CancellationToken cancellationToken)
    {
        _configurations.Add(configuration);

        return Task.CompletedTask;
    }

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

    public Task Update(ConfigurationModel configuration, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
