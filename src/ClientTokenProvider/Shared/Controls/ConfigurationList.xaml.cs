using ClientTokenProvider.Business.Shared.Models;
using CommunityToolkit.Mvvm.Input;

namespace ClientTokenProvider.Shared.Controls;

public partial class ConfigurationList : ContentView
{
    public static readonly BindableProperty ConfigurationsProperty = BindableProperty.Create(
        nameof(Configurations),
        typeof(IEnumerable<ConfigurationModel>),
        typeof(ConfigurationList));

    public static readonly BindableProperty SelectedConfigurationProperty = BindableProperty.Create(
        nameof(SelectedConfiguration),
        typeof(ConfigurationModel),
        typeof(ConfigurationList));

    public static readonly BindableProperty AddNewConfigurationCommandProperty = BindableProperty.Create(
        nameof(AddNewConfigurationCommand),
        typeof(IAsyncRelayCommand<ConfigurationKind>),
        typeof(ConfigurationList));

    public static readonly BindableProperty SetActiveConfigurationCommandProperty = BindableProperty.Create(
        nameof(SetActiveConfigurationCommand),
        typeof(IRelayCommand<ConfigurationModel>),
        typeof(ConfigurationList));

    public IEnumerable<ConfigurationModel> Configurations
    {
        get => (IEnumerable<ConfigurationModel>)GetValue(ConfigurationsProperty);
        set => SetValue(ConfigurationsProperty, value);
    }

    public ConfigurationModel? SelectedConfiguration
    {
        get => (ConfigurationModel?)GetValue(SelectedConfigurationProperty);
        set => SetValue(SelectedConfigurationProperty, value);
    }

    public IAsyncRelayCommand<ConfigurationKind> AddNewConfigurationCommand
    {
        get => (IAsyncRelayCommand<ConfigurationKind>)GetValue(AddNewConfigurationCommandProperty);
        set => SetValue(AddNewConfigurationCommandProperty, value);
    }

    public IRelayCommand<ConfigurationModel> SetActiveConfigurationCommand
    {
        get => (IRelayCommand<ConfigurationModel>)GetValue(SetActiveConfigurationCommandProperty);
        set => SetValue(SetActiveConfigurationCommandProperty, value);
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
        var configuration = (ConfigurationModel?)e.SelectedItem;

        if (configuration is null)
        {
            return;
        }

        SetActiveConfigurationCommand?.Execute(configuration);
    }
}