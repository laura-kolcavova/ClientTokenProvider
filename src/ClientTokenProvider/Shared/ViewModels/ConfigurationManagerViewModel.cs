using ClientTokenProvider.Shared.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ClientTokenProvider.Shared.ViewModels;

public partial class ConfigurationManagerViewModel :
    ObservableObject,
    IRecipient<ConfigurationSelectedMessage>
{
    [ObservableProperty]
    private Guid? currentConfigurationId;

    public void Receive(ConfigurationSelectedMessage message)
    {
        CurrentConfigurationId = message.ConfigurationIdentity.Id;
    }

    [RelayCommand]
    private void Load()
    {
        WeakReferenceMessenger.Default.RegisterAll(this);
    }

    [RelayCommand]
    private void Unload()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
}
