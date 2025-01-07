using ClientTokenProvider.Shared.Messages;
using ClientTokenProvider.Shared.ViewModels;
using CommunityToolkit.Mvvm.Messaging;

namespace ClientTokenProvider.Shared.Views;

public partial class ConfigurationManagerPage :
    ContentPage,
    IRecipient<ShowSavingFileFailedErrorMessage>
{
    public ConfigurationManagerPage(ConfigurationManagerViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;

        Loaded += (s, e) =>
        {
            WeakReferenceMessenger.Default.RegisterAll(this);

            viewModel.LoadCommand.Execute(null);
        };

        Unloaded += (s, e) =>
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);

            viewModel.UnloadCommand.Execute(null);
        };
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