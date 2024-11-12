using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Business.Shared.Providers;

namespace ClientTokenProvider.Business.Shared.Services;

internal sealed class ConfigurationRepository(
    IConfigurationIdentityProvider configurationIdentityProvider,
    IConfigurationCache configurationCache) : IConfigurationRepository
{
    public async Task<Configuration<TConfigurationDataType>> Create<TConfigurationDataType>(
         Func<TConfigurationDataType> configurationDataFactory,
         CancellationToken cancellationToken)
        where TConfigurationDataType : notnull
    {
        var identity = configurationIdentityProvider.NewIdentity();
        var data = configurationDataFactory();

        var configuration = new Configuration<TConfigurationDataType>()
        {
            Identity = identity,
            Data = data
        };

        // TODO Store it in the database
        await Task.CompletedTask;

        configurationCache.Save(configuration);

        return configuration;
    }

    public async ValueTask<Configuration<TConfigurationDataType>?> Get<TConfigurationDataType>(
        Guid configurationGuid,
        CancellationToken cancellationToken)
        where TConfigurationDataType : notnull
    {
        var configuration = configurationCache.Get<TConfigurationDataType>(
            configurationGuid);

        if (configuration is null)
        {
            // TODO Obtain data from DB
            await Task.CompletedTask;
        }

        return configuration;
    }

    public async ValueTask<IReadOnlyCollection<ConfigurationBase>> GetAll(
        CancellationToken cancellationToken)
    {
        var configurations = configurationCache.GetAll();

        if (configurations is null)
        {
            // TODO Obtain data from DB
            await Task.CompletedTask;
        }

        return configurations ?? new List<ConfigurationBase>();
    }

    public async Task Save(
        ConfigurationBase configuration,
        CancellationToken cancellationToken)
    {
        // TODO Store it in the database
        await Task.CompletedTask;

        configurationCache.Save(configuration);
    }
}
