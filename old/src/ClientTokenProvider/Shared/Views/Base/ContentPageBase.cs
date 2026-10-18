using ClientTokenProvider.Shared.ViewModels.Base;

namespace ClientTokenProvider.Shared.Views.Base;

public abstract class ContentPageBase : ContentPage
{
    protected ContentPageBase() { }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ViewModelBase viewModel)
        {
            await viewModel.LoadCommand.ExecuteAsync(null);
        }
    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();

        if (BindingContext is ViewModelBase viewModel)
        {
            await viewModel.UnloadCommand.ExecuteAsync(null);
        }
    }
}
