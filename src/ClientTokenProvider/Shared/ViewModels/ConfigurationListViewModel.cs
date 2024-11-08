using ClientTokenProvider.Shared.Managers;
using ClientTokenProvider.Shared.Messages;
using ClientTokenProvider.Shared.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace ClientTokenProvider.Shared.ViewModels;

public partial class ConfigurationListViewModel : ObservableObject,
    IRecipient<ConfigurationSavedMessage>,
    IRecipient<ConfigurationNameChangedMessage>
{
    private readonly IConfigurationIdentityManager _configurationIdentityManager;

    [ObservableProperty]
    private ObservableCollection<ConfigurationListItemModel> configurationList;

    [ObservableProperty]
    private ConfigurationListItemModel? selectedItem;

    [ObservableProperty]
    private bool isItemSelected;

    public ConfigurationListViewModel(
        IConfigurationIdentityManager configurationIdentityManager)
    {
        _configurationIdentityManager = configurationIdentityManager;

        ConfigurationList = [];
    }

    public void Receive(ConfigurationSavedMessage message)
    {
        if (ConfigurationList
           .Any(configurationListItem => configurationListItem.Id == message.ConfigurationIdentity.Id))
        {
            return;
        }

        AddConfiguration(message.ConfigurationIdentity);
    }

    public void Receive(ConfigurationNameChangedMessage message)
    {
        var index = -1;

        for (var i = 0; i < ConfigurationList.Count; i++)
        {
            if (ConfigurationList[i].Id == message.ConfigurationIdentity.Id)
            {
                index = i;
                break;
            }
        }

        if (index > -1)
        {
            var newConfigurationListItem = new ConfigurationListItemModel
            {
                Id = message.ConfigurationIdentity.Id,
                Name = message.ConfigurationIdentity.Name,
            };

            ConfigurationList[index] = newConfigurationListItem;
        }
    }

    partial void OnSelectedItemChanged(ConfigurationListItemModel? value)
    {
        if (value is null)
        {
            IsItemSelected = false;

            WeakReferenceMessenger.Default.Send(new ConfigurationUnselectedMessage());
        }
        else
        {
            IsItemSelected = true;

            var configurationIdentity = new ConfigurationIdentityModel
            {
                Id = value.Id,
                Name = value.Name,
            };

            WeakReferenceMessenger.Default.Send(new ConfigurationSelectedMessage
            {
                ConfigurationIdentity = configurationIdentity,
            });
        }
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

    [RelayCommand]
    private void SelectOrDeselectItem(ConfigurationListItemModel item)
    {
        if (!ConfigurationList.Contains(item))
        {
            return;
        }

        if (SelectedItem == item)
        {
            SelectedItem = null;
        }
        else
        {
            SelectedItem = item;
        }
    }

    [RelayCommand]
    private void AddNewConfiguration()
    {
        var configurationIdentity = _configurationIdentityManager
            .NewIdentity();

        AddConfiguration(configurationIdentity);
    }

    private void AddConfiguration(ConfigurationIdentityModel configurationIdentity)
    {
        var newConfigurationListItem = new ConfigurationListItemModel
        {
            Id = configurationIdentity.Id,
            Name = configurationIdentity.Name,
        };

        ConfigurationList.Add(newConfigurationListItem);
        SelectedItem = newConfigurationListItem;
    }
}
