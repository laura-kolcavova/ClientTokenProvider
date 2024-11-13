using ClientTokenProvider.AzureAd.Messages;
using ClientTokenProvider.AzureAd.ViewModels;
using CommunityToolkit.Mvvm.Messaging;

namespace ClientTokenProvider.AzureAd.Views;

public partial class ConfigurationDetailView :
    ContentPage,
    IRecipient<ShowErrorDetailMessage>,
    IRecipient<ShowSavingFileFailedErrorMessage>
{
    public ConfigurationDetailView(ConfigurationDetailViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;

        Loaded += (s, e) =>
        {
            WeakReferenceMessenger.Default.RegisterAll(this);

            AuthorityEntry.Focus();

            viewModel.LoadCommand.Execute(null);
        };

        Unloaded += (s, e) =>
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);

            viewModel.UnloadCommand.Execute(null);
        };
    }

    async void IRecipient<ShowErrorDetailMessage>.Receive(
        ShowErrorDetailMessage message)
    {
        await DisplayAlert(
            "Error",
            message.ErrorMessage,
            "Close");
    }

    async void IRecipient<ShowSavingFileFailedErrorMessage>.Receive(
        ShowSavingFileFailedErrorMessage message)
    {
        await DisplayAlert(
            "Saving file failed",
            "File could not be saved",
            "Close");
    }
}