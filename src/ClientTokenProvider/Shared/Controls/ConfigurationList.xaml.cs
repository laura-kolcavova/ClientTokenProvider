using ClientTokenProvider.Business.Shared.Models;
using CommunityToolkit.Mvvm.Input;

namespace ClientTokenProvider.Shared.Controls;

public partial class ConfigurationList : ContentView
{
    public static readonly BindableProperty ConfigurationsProperty = BindableProperty.Create(
        nameof(Configurations),
        typeof(IEnumerable<Configuration>),
        typeof(ConfigurationList));

    public static readonly BindableProperty SelectedConfigurationProperty = BindableProperty.Create(
        nameof(SelectedConfiguration),
        typeof(Configuration),
        typeof(ConfigurationList));

    public static readonly BindableProperty AddNewConfigurationCommandProperty = BindableProperty.Create(
        nameof(AddNewConfigurationCommand),
        typeof(IAsyncRelayCommand<ConfigurationKind>),
        typeof(ConfigurationList));

    public static readonly BindableProperty SetActiveConfigurationCommandProperty = BindableProperty.Create(
        nameof(SetActiveConfigurationCommand),
        typeof(IRelayCommand<Configuration>),
        typeof(ConfigurationList));

    public IEnumerable<Configuration> Configurations
    {
        get => (IEnumerable<Configuration>)GetValue(ConfigurationsProperty);
        set => SetValue(ConfigurationsProperty, value);
    }

    public Configuration? SelectedConfiguration
    {
        get => (Configuration?)GetValue(SelectedConfigurationProperty);
        set => SetValue(SelectedConfigurationProperty, value);
    }

    public IAsyncRelayCommand<ConfigurationKind> AddNewConfigurationCommand
    {
        get => (IAsyncRelayCommand<ConfigurationKind>)GetValue(AddNewConfigurationCommandProperty);
        set => SetValue(AddNewConfigurationCommandProperty, value);
    }

    public IRelayCommand<Configuration> SetActiveConfigurationCommand
    {
        get => (IRelayCommand<Configuration>)GetValue(SetActiveConfigurationCommandProperty);
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
        var configuration = (Configuration?)e.SelectedItem;

        if (configuration is null)
        {
            return;
        }

        SetActiveConfigurationCommand?.Execute(configuration);
    }
}