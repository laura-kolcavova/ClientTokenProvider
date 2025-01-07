using ClientTokenProvider.Business.Shared.Factories;
using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Business.Shared.Services;

internal sealed class ConfigurationService(
    IConfigurationFactory configurationFactory,
    IConfigurationCache configurationCache) :
    IConfigurationService
{
    public async Task<Configuration> Create(
        ConfigurationKind kind,
        CancellationToken cancellationToken)
    {
        var configuration = configurationFactory.Create(kind);

        // TODO Store it in the database
        await Task.CompletedTask;

        configurationCache.Save(configuration);

        return configuration;
    }

    public async ValueTask<Configuration?> Get(
        Guid configurationId,
        CancellationToken cancellationToken)
    {
        var configuration = configurationCache.Get(
            configurationId);

        if (configuration is null)
        {
            // TODO Obtain data from DB
            await Task.CompletedTask;
        }

        return configuration;
    }

    public async ValueTask<IReadOnlyCollection<Configuration>> GetAll(
        CancellationToken cancellationToken)
    {
        var configurations = configurationCache.GetAll();

        if (configurations is null)
        {
            // TODO Obtain data from DB
            await Task.CompletedTask;
        }

        return configurations ?? new List<Configuration>();
    }

    public async Task Save(
        Configuration configuration,
        CancellationToken cancellationToken)
    {
        // TODO Store it in the database
        await Task.CompletedTask;

        configurationCache.Save(configuration);
    }
}
