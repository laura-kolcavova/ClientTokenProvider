using ClientTokenProvider.Shared.Messages;
using ClientTokenProvider.Shared.ViewModels;
using ClientTokenProvider.Shared.Views.Base;
using CommunityToolkit.Mvvm.Messaging;

namespace ClientTokenProvider.Shared.Views;

public partial class ConfigurationManagerPage :
    ContentPageBase,
    IRecipient<ShowSavingFileFailedErrorMessage>
{
    public ConfigurationManagerPage(ConfigurationManagerViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        WeakReferenceMessenger.Default.RegisterAll(this);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        WeakReferenceMessenger.Default.UnregisterAll(this);
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