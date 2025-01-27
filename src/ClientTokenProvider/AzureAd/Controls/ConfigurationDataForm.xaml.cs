using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.AzureAd.Controls;

public partial class ConfigurationDataForm : ContentView
{
    public static readonly BindableProperty ConfigurationProperty = BindableProperty.Create(
        nameof(Configuration),
        typeof(ConfigurationModel),
        typeof(ConfigurationDataForm));

    public ConfigurationModel Configuration
    {
        get => (ConfigurationModel)GetValue(ConfigurationProperty);
        set => SetValue(ConfigurationProperty, value);
    }

    public ConfigurationDataForm()
    {
        InitializeComponent();
    }
}