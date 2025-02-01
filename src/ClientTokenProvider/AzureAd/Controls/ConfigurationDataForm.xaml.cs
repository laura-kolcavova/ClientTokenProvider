using ClientTokenProvider.AzureAd.BindableViewModels;

namespace ClientTokenProvider.AzureAd.Controls;

public partial class ConfigurationDataForm : ContentView
{
    public static readonly BindableProperty ConfigurationDataProperty = BindableProperty.Create(
        nameof(ConfigurationData),
        typeof(ConfigurationDataBindableModel),
        typeof(ConfigurationDataForm));

    public ConfigurationDataBindableModel ConfigurationData
    {
        get => (ConfigurationDataBindableModel)GetValue(ConfigurationDataProperty);
        set => SetValue(ConfigurationDataProperty, value);
    }

    public ConfigurationDataForm()
    {
        InitializeComponent();
    }
}