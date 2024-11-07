using ClientTokenProvider.Shared.Messages;
using ClientTokenProvider.Shared.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ClientTokenProvider.Shared.ViewModels;

public partial class ConfigurationListViewModel : ObservableObject
{
    [ObservableProperty]
    private IReadOnlyCollection<ConfigurationListItemModel> configurationList;

    [ObservableProperty]
    private ConfigurationListItemModel? selectedItem;

    [ObservableProperty]
    private bool isItemSelected;

    public ConfigurationListViewModel()
    {
        ConfigurationList = [];
    }

    partial void OnSelectedItemChanged(ConfigurationListItemModel? value)
    {
        if (value is null)
        {
            IsItemSelected = false;
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
    private void SelectItem(ConfigurationListItemModel item)
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
}
