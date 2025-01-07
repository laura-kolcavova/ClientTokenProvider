using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Business.Shared.Services;
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
    private readonly IConfigurationService _configurationService;

    [ObservableProperty]
    private ObservableCollection<ConfigurationListItemModel> configurationList;

    [ObservableProperty]
    private ConfigurationListItemModel? selectedItem;

    [ObservableProperty]
    private bool isItemSelected;

    public ConfigurationListViewModel(
        IConfigurationService configurationService)
    {
        _configurationService = configurationService;

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
        if (!message.IsConfigurationSaved)
        {
            return;
        }

        UpdateConfiguration(message.ConfigurationIdentity);
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

            var configurationIdentity = new ConfigurationIdentity
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
    private async Task Load(CancellationToken cancellationToken)
    {
        WeakReferenceMessenger.Default.RegisterAll(this);

        var configurations = await _configurationService
            .GetAll(cancellationToken);

        var configurationListItems = configurations
            .Select(configuration => new ConfigurationListItemModel
            {
                Id = configuration.Identity.Id,
                Name = configuration.Identity.Name,
            });

        ConfigurationList = new ObservableCollection<ConfigurationListItemModel>(
           configurationListItems);
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

    [RelayCommand(
        IncludeCancelCommand = true,
        AllowConcurrentExecutions = false)]
    private async Task AddNewConfiguration(CancellationToken cancellationToken)
    {
        var configuration = await _configurationService
            .Create(
                ConfigurationKind.AzureAd,
                cancellationToken);

        AddConfiguration(configuration.Identity);
    }

    private void AddConfiguration(ConfigurationIdentity configurationIdentity)
    {
        var newConfigurationListItem = new ConfigurationListItemModel
        {
            Id = configurationIdentity.Id,
            Name = configurationIdentity.Name,
        };

        ConfigurationList.Add(newConfigurationListItem);
        SelectedItem = newConfigurationListItem;
    }

    private void UpdateConfiguration(ConfigurationIdentity configurationIdentity)
    {
        var index = -1;

        for (var i = 0; i < ConfigurationList.Count; i++)
        {
            if (ConfigurationList[i].Id == configurationIdentity.Id)
            {
                index = i;
                break;
            }
        }

        if (index > -1)
        {
            var newConfigurationListItem = new ConfigurationListItemModel
            {
                Id = configurationIdentity.Id,
                Name = configurationIdentity.Name,
            };

            ConfigurationList[index] = newConfigurationListItem;
        }
    }
}