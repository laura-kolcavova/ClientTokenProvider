using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Shared.BindableModels;
using CommunityToolkit.Mvvm.Input;

namespace ClientTokenProvider.Shared.Controls;

public partial class ConfigurationList : ContentView
{
    public static readonly BindableProperty ConfigurationListItemsProperty = BindableProperty.Create(
        nameof(ConfigurationListItems),
        typeof(IEnumerable<ConfigurationListItemBindableModel>),
        typeof(ConfigurationList));

    public static readonly BindableProperty ActiveConfigurationListItemProperty = BindableProperty.Create(
        nameof(ActiveConfigurationListItem),
        typeof(ConfigurationListItemBindableModel),
        typeof(ConfigurationList));

    public static readonly BindableProperty AddNewConfigurationCommandProperty = BindableProperty.Create(
        nameof(AddNewConfigurationCommand),
        typeof(IRelayCommand<ConfigurationKind>),
        typeof(ConfigurationList));

    public static readonly BindableProperty SetActiveConfigurationListItemCommandProperty = BindableProperty.Create(
        nameof(SetActiveConfigurationListItemCommand),
        typeof(IRelayCommand<ConfigurationListItemBindableModel>),
        typeof(ConfigurationList));

    public IEnumerable<ConfigurationListItemBindableModel> ConfigurationListItems
    {
        get => (IEnumerable<ConfigurationListItemBindableModel>)GetValue(ConfigurationListItemsProperty);
        set => SetValue(ConfigurationListItemsProperty, value);
    }

    public ConfigurationListItemBindableModel? ActiveConfigurationListItem
    {
        get => (ConfigurationListItemBindableModel?)GetValue(ActiveConfigurationListItemProperty);
        set => SetValue(ActiveConfigurationListItemProperty, value);
    }

    public IRelayCommand<ConfigurationKind> AddNewConfigurationCommand
    {
        get => (IRelayCommand<ConfigurationKind>)GetValue(AddNewConfigurationCommandProperty);
        set => SetValue(AddNewConfigurationCommandProperty, value);
    }

    public IRelayCommand<ConfigurationListItemBindableModel> SetActiveConfigurationListItemCommand
    {
        get => (IRelayCommand<ConfigurationListItemBindableModel>)GetValue(SetActiveConfigurationListItemCommandProperty);
        set => SetValue(SetActiveConfigurationListItemCommandProperty, value);
    }

    public ConfigurationList()
    {
        InitializeComponent();
    }

    private void AddNewConfigurationButton_Clicked(object sender, EventArgs e)
    {
        AddNewConfigurationCommand?.Execute(ConfigurationKind.AzureAd);
    }

    private void ConfigurationListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var configurationListItem = (ConfigurationListItemBindableModel?)e.SelectedItem;

        if (configurationListItem is null)
        {
            return;
        }

        SetActiveConfigurationListItemCommand?.Execute(configurationListItem);
    }
}