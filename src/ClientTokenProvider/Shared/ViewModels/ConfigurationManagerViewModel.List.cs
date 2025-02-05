using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Shared.BindableModels;
using ClientTokenProvider.Shared.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
}
