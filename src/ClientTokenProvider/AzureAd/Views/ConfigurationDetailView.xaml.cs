using ClientTokenProvider.AzureAd.Messages;
using ClientTokenProvider.AzureAd.ViewModels;
using CommunityToolkit.Mvvm.Messaging;

namespace ClientTokenProvider.AzureAd.Views;

public partial class ConfigurationDetailView :
    ContentPage,
    IRecipient<ShowErrorDetailMessage>
{
    public ConfigurationDetailView(ConfigurationDetailViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();

        Loaded += (s, e) =>
        {
            WeakReferenceMessenger.Default.RegisterAll(this);
        };

        Unloaded += (s, e) =>
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
        };
    }

    async void IRecipient<ShowErrorDetailMessage>.Receive(ShowErrorDetailMessage message)
    {
        await DisplayAlert("Error", message.ErrorMessage, "Close");
    }
}