using ClientTokenProvider.Shared.Models;
using CommunityToolkit.Maui.Views;

namespace ClientTokenProvider.Shared.Popups;

public partial class SaveChangesBeforeClosePopup : Popup
{
    public SaveChangesBeforeClosePopup()
    {
        InitializeComponent();
    }

    private async void DontSaveButton_Clicked(object sender, EventArgs e)
    {
        await Close(
            SaveChangesBeforeExitPopupResult.ExitWithoutSave);
    }

    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        await Close(
            SaveChangesBeforeExitPopupResult.Cancel);
    }

    private async void SaveChangesButton_Clicked(object sender, EventArgs e)
    {
        await Close(
            SaveChangesBeforeExitPopupResult.SaveAndExit);
    }

    private async Task Close(SaveChangesBeforeExitPopupResult result)
    {
        using var cancellationTokenSource = new CancellationTokenSource(
            TimeSpan.FromSeconds(5));

        await CloseAsync(
            result,
            cancellationTokenSource.Token);
    }
}