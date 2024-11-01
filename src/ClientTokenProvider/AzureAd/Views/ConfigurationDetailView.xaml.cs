using ClientTokenProvider.AzureAd.ViewModels;

namespace ClientTokenProvider.AzureAd.Views;

public partial class ConfigurationDetailView : ContentPage
{
    public ConfigurationDetailView(ConfigurationDetailViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}