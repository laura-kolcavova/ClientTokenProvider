using ClientTokenProvider.Business.Shared.Factories;
using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Business.Shared.Repositories;

namespace ClientTokenProvider.Business.Shared.Services;

internal sealed class ConfigurationService(
    IConfigurationFactory configurationFactory,
    IConfigurationCacheService configurationCacheService,
    IConfigurationRepository configurationRepository) :
    IConfigurationService
{
    public async Task<ConfigurationModel> Create(
        ConfigurationKind kind,
        CancellationToken cancellationToken)
    {
        var configuration = configurationFactory.Create(kind);

        configurationCacheService.Update(configuration);

        // TODO Store it in the database
        await Task.CompletedTask;

        return configuration;
    }

    public async Task Delete
        (Guid configurationId,
        CancellationToken cancellationToken)
    {
        configurationCacheService.Remove(configurationId);

        // TODO Remove it in the database
        await Task.CompletedTask;
    }

    public async ValueTask<ConfigurationModel?> Get(
        Guid configurationId,
        CancellationToken cancellationToken)
    {
        var configuration = configurationCacheService.Get(
            configurationId);

        if (configuration is null)
        {
            configuration = await configurationRepository.Get(
                configurationId,
                cancellationToken);

            if (configuration is not null)
            {
                configurationCacheService.Add(configuration);
            }
        }

        return configuration;
    }

    public async ValueTask<IReadOnlyCollection<ConfigurationModel>> GetAll(
        CancellationToken cancellationToken)
    {
        var configurations = configurationCacheService.GetAll();

        if (configurations is null)
        {
            configurations = await configurationRepository.GetAll(
                cancellationToken);

            configurationCacheService.AddMany(configurations);
        }

        return configurations;
    }

    public async Task Save(
        ConfigurationModel configuration,
        CancellationToken cancellationToken)
    {
        configurationCacheService.Update(configuration);

        // TODO Store it in the database
        await Task.CompletedTask;
    }
}
