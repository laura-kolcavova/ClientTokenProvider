using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Shared.BindableModels;
using ClientTokenProvider.Shared.Extensions;
using ClientTokenProvider.Shared.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace ClientTokenProvider.Shared.ViewModels;

public partial class ConfigurationManagerViewModel
{
    [ObservableProperty]
    private ObservableCollection<ConfigurationListItemBindableModel> _configurationListItems = [];

    [ObservableProperty]
    private ConfigurationListItemBindableModel? _activeConfigurationListItem;

    [RelayCommand]
    private void SetActiveConfigurationListItem(ConfigurationListItemBindableModel configurationListItem)
    {
        SetActiveConfigurationListItem_Internal(configurationListItem);
        SetActiveConfigurationDetail_Internal(configurationListItem.Id);
    }

    [RelayCommand(
       IncludeCancelCommand = true,
       AllowConcurrentExecutions = false)]
    private async Task RemoveConfigurationListItem(
        ConfigurationListItemBindableModel configurationListItem,
        CancellationToken cancellationToken)
    {
        var removeResult = await RemoveConfiguration_Internal(
            configurationListItem.Id,
            cancellationToken);

        if (removeResult.IsFailure)
        {
            WeakReferenceMessenger.Default.Send(
                new DeletingConfigurationFailedMessage());

            return;
        }

        RemoveConfigurationListItem_Internal(configurationListItem);
        RemoveConfigurationDetail_Internal(configurationListItem.Id);
    }

    [RelayCommand(
       IncludeCancelCommand = true,
       AllowConcurrentExecutions = false)]
    private async Task ImportConfiguration(
        CancellationToken cancellationToken)
    {
        var importResult = await ImportConfiguration_Internal(
            cancellationToken);

        if (importResult.IsFailure)
        {
            // TODO Message
            return;
        }

        CreateAndAddConfigurationListItem_Internal(importResult.Value);
    }

    private ConfigurationListItemBindableModel CreateAndAddConfigurationListItem_Internal(
       ConfigurationModel configuration)
    {
        var configurationListItem = configuration.ToListItem();

        ConfigurationListItems.Add(configurationListItem);

        return configurationListItem;
    }

    private void SetActiveConfigurationListItem_Internal(
        ConfigurationListItemBindableModel configurationListItem)
    {
        ActiveConfigurationListItem = configurationListItem;
    }

    private void RenameConfigurationListItem_Internal(
        Guid configurationId,
        string newName)
    {
        var configurationListItem = ConfigurationListItems.FirstOrDefault(configurationListItem =>
            configurationListItem.Id == configurationId);

        if (configurationListItem is not null)
        {
            configurationListItem.Name = newName;
        }
    }

    private void RemoveConfigurationListItem_Internal(
        ConfigurationListItemBindableModel configurationListItem)
    {
        ConfigurationListItems.Remove(configurationListItem);
    }
}
