using ClientTokenProvider.Shared.Helpers;
using ClientTokenProvider.Shared.ViewModels;

namespace ClientTokenProvider.Shared.Views;

public partial class ConfigurationListView : ContentView
{
    public ConfigurationListView()
    {
        InitializeComponent();

        var viewModel = ServiceHelper.GetRequiredService<ConfigurationListViewModel>();

        BindingContext = viewModel;
    }
}