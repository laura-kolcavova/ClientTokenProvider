using ClientTokenProvider.Shared.BindableModels.Abstractions;

namespace ClientTokenProvider.Shared.Services.Abstractions;

public interface IConfigurationDataBackupStore
{
    public void Set(
        Guid configurationId,
        IConfigurationDataBindableModel configurationData);

    public bool IsSet(
        Guid configurationId);

    public void Delete(
        Guid configurationId);

    public bool AnyChanges(
        Guid configurationId,
        IConfigurationDataBindableModel configurationData);
}
