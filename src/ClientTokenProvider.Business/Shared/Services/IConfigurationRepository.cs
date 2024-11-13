using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Business.Shared.Services;

public interface IConfigurationRepository
{
    public Task<Configuration<TConfigurationDataType>> Create<TConfigurationDataType>(
        Func<TConfigurationDataType> configurationDataFactory,
        CancellationToken cancellationToken)
        where TConfigurationDataType : notnull;

    public Task Save(
        ConfigurationBase configurationBase,
        CancellationToken cancellationToken);

    public ValueTask<IReadOnlyCollection<ConfigurationBase>> GetAll(
        CancellationToken cancellationToken);

    public ValueTask<Configuration<TConfigurationDataType>?> Get<TConfigurationDataType>(
       Guid configurationGuid,
       CancellationToken cancellationToken)
       where TConfigurationDataType : notnull;
}
