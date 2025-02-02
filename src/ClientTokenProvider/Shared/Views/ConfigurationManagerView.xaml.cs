using ClientTokenProvider.Shared.Messages;
using ClientTokenProvider.Shared.ViewModels;
using ClientTokenProvider.Shared.Views.Base;
using CommunityToolkit.Mvvm.Messaging;

namespace ClientTokenProvider.Shared.Views;

public partial class ConfigurationManagerPage :
    ContentPageBase,
    IRecipient<RenamingConfigurationFailedMessage>,
    IRecipient<DeletingConfigurationFailedMessage>,
    IRecipient<SavingConfigurationDataFailedMessage>
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

    public async void Receive(RenamingConfigurationFailedMessage message)
    {
        // TODO Localization
        await DisplayAlert(
            "Renaming configuration failed",
            "Configuration could not be renamed",
            "Close");
    }

    public async void Receive(DeletingConfigurationFailedMessage message)
    {
        // TODO Localization
        await DisplayAlert(
            "Deleting configuration failed",
            "Configuration could not be deleted",
            "Close");
    }

    public async void Receive(SavingConfigurationDataFailedMessage message)
    {
        // TODO Localization
        await DisplayAlert(
            "Saving configuration data failed",
            "Configuration data could not be saved",
            "Close");
    }
}