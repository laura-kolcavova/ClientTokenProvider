using ClientTokenProvider.AzureAd.BindableViewModels;
using CommunityToolkit.Mvvm.Input;

namespace ClientTokenProvider.AzureAd.Controls;

public partial class ConfigurationDataForm : ContentView
{
    public static readonly BindableProperty ConfigurationDataProperty = BindableProperty.Create(
        nameof(ConfigurationData),
        typeof(ConfigurationDataBindableModel),
        typeof(ConfigurationDataForm));

    public static readonly BindableProperty HandleConfigurationDataChangedCommandProperty = BindableProperty.Create(
       nameof(HandleConfigurationDataChangedCommand),
       typeof(IRelayCommand),
       typeof(ConfigurationDataForm));

    public ConfigurationDataBindableModel ConfigurationData
    {
        get => (ConfigurationDataBindableModel)GetValue(ConfigurationDataProperty);
        set => SetValue(ConfigurationDataProperty, value);
    }

    public IRelayCommand HandleConfigurationDataChangedCommand
    {
        get => (IRelayCommand)GetValue(HandleConfigurationDataChangedCommandProperty);
        set => SetValue(HandleConfigurationDataChangedCommandProperty, value);
    }

    public ConfigurationDataForm()
    {
        InitializeComponent();
    }

    private void DataEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        HandleConfigurationDataChangedCommand?.Execute(null);
    }
}