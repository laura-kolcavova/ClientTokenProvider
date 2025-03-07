using ClientTokenProvider.Shared.Messages;
using ClientTokenProvider.Shared.Models;
using ClientTokenProvider.Shared.Popups;
using ClientTokenProvider.Shared.ViewModels;
using ClientTokenProvider.Shared.Views.Base;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;

namespace ClientTokenProvider.Shared.Views;

public partial class ConfigurationManagerPage :
    ContentPageBase,
    IRecipient<GettingConfigurationsFailedMessage>,
    IRecipient<AddingConfigurationFailedMessage>,
    IRecipient<RenamingConfigurationFailedMessage>,
    IRecipient<SavingConfigurationDataFailedMessage>,
    IRecipient<ExportingConfigurationFailedMessage>,
    IRecipient<ImportingConfigurationFailedMessage>,
    IRecipient<DeletingConfigurationFailedMessage>,
    IRecipient<ShowAccessTokenErrorDetailMessage>,
    IRecipient<ShowSaveChangesBeforeCloseDetailMessage>
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

    public async void Receive(GettingConfigurationsFailedMessage message)
    {
        // TODO Localization
        await DisplayAlert(
            "Getting configurations failed",
            "Configurations could not be get",
            "Close");
    }

    public async void Receive(AddingConfigurationFailedMessage message)
    {
        // TODO Localization
        await DisplayAlert(
            "Adding configuration failed",
            "Configuration could not be added",
            "Close");
    }

    public async void Receive(RenamingConfigurationFailedMessage message)
    {
        // TODO Localization
        await DisplayAlert(
            "Renaming configuration failed",
            "Configuration could not be renamed",
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

    public async void Receive(ExportingConfigurationFailedMessage message)
    {
        await DisplayAlert(
            "Exporting configuration failed",
            "Configuration could not be exported",
            "Close");
    }

    public async void Receive(ImportingConfigurationFailedMessage message)
    {
        await DisplayAlert(
            "Importing configuration failed",
            "Configuration could not be imported",
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

    public async void Receive(ShowAccessTokenErrorDetailMessage message)
    {
        await DisplayAlert(
            "Error",
            message.ErrorMessage,
            "Close");
    }

    public async void Receive(ShowSaveChangesBeforeCloseDetailMessage message)
    {
        var popup = new SaveChangesBeforeClosePopup();

        var resultObject = await this.ShowPopupAsync(
            popup,
            CancellationToken.None);

        var result = resultObject is null
            ? SaveChangesBeforeExitPopupResult.Close
            : (SaveChangesBeforeExitPopupResult)resultObject;

        WeakReferenceMessenger
            .Default
            .Send(new HandlePopupResultMessage<SaveChangesBeforeExitPopupResult>
            {
                Result = result
            });
    }
}