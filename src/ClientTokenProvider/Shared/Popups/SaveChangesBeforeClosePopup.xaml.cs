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
            SaveChangesBeforeClosePopupResult.DontSave);
    }

    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        await Close(
            SaveChangesBeforeClosePopupResult.Cancel);
    }

    private async void SaveChangesButton_Clicked(object sender, EventArgs e)
    {
        await Close(
            SaveChangesBeforeClosePopupResult.SaveChanges);
    }

    private async Task Close(SaveChangesBeforeClosePopupResult result)
    {
        using var cancellationTokenSource = new CancellationTokenSource(
            TimeSpan.FromSeconds(5));

        await CloseAsync(
            result,
            cancellationTokenSource.Token);
    }
}