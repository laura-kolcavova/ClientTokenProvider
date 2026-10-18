using ClientTokenProvider.AzureAd.BindableViewModels;
using CommunityToolkit.Mvvm.Input;

namespace ClientTokenProvider.AzureAd.Controls;

public partial class AzureAdConfigurationDataForm : ContentView
{
    public static readonly BindableProperty ConfigurationDataProperty = BindableProperty.Create(
        nameof(ConfigurationData),
        typeof(AzureAdConfigurationDataBindableModel),
        typeof(AzureAdConfigurationDataForm));

    public static readonly BindableProperty HandleConfigurationDataChangedCommandProperty = BindableProperty.Create(
       nameof(HandleConfigurationDataChangedCommand),
       typeof(IRelayCommand),
       typeof(AzureAdConfigurationDataForm));

    public AzureAdConfigurationDataBindableModel ConfigurationData
    {
        get => (AzureAdConfigurationDataBindableModel)GetValue(ConfigurationDataProperty);
        set => SetValue(ConfigurationDataProperty, value);
    }

    public IRelayCommand HandleConfigurationDataChangedCommand
    {
        get => (IRelayCommand)GetValue(HandleConfigurationDataChangedCommandProperty);
        set => SetValue(HandleConfigurationDataChangedCommandProperty, value);
    }

    public AzureAdConfigurationDataForm()
    {
        InitializeComponent();
    }

    private void DataEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        HandleConfigurationDataChangedCommand?.Execute(null);
    }
}