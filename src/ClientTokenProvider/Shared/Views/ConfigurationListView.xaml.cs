using ClientTokenProvider.Shared.ViewModels;

namespace ClientTokenProvider.Shared.Views;

public partial class ConfigurationListView : ContentView
{
    public ConfigurationListView()
    {
        InitializeComponent();

        var viewModel = Handler!.MauiContext!.Services.GetRequiredService<ConfigurationListViewModel>();

        //var viewModel = ServiceHelper.GetRequiredService<ConfigurationListViewModel>();

        BindingContext = viewModel;

        Loaded += (s, e) =>
        {
            viewModel.LoadCommand.Execute(null);
        };

        Unloaded += (s, e) =>
        {
            viewModel.UnloadCommand.Execute(null);
        };
    }
}