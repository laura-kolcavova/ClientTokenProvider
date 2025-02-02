using ClientTokenProvider.Shared.BindableModels.Abstractions;
using ClientTokenProvider.Shared.Services.Abstractions;

namespace ClientTokenProvider.Shared.Services;

internal sealed class ConfigurationDataBackupStore :
    IConfigurationDataBackupStore
{
    private readonly Dictionary<Guid, IConfigurationDataBindableModel> _store = [];

    public void Delete(Guid configurationId)
    {
        _store.Remove(configurationId);
    }

    public bool AnyChanges(
        Guid configurationId,
        IConfigurationDataBindableModel configurationData)
    {
        if (!_store.TryGetValue(configurationId, out var configurationDataBackup))
        {
            // If there is no backup, assume no changes exist.
            return false;
        }

        if (configurationDataBackup.GetType() != configurationData.GetType())
        {
            throw new ArgumentException(
                $"Mismatched configuration types. Expected: {configurationDataBackup.GetType().FullName}, but received: {configurationData.GetType().FullName}.",
                nameof(configurationData));
        }

        var dataComponentsA = configurationData
            .GetDataComponents();

        var dataComponentsB = configurationDataBackup
            .GetDataComponents();

        return !dataComponentsA.SequenceEqual(dataComponentsB);
    }

    public void Set(Guid configurationId, IConfigurationDataBindableModel configurationData)
    {
        _store[configurationId] = configurationData.Copy();
    }

    public bool IsSet(Guid configurationId)
    {
        return _store.ContainsKey(configurationId);
    }
}
