using KittyToken.Maui.Models;

namespace KittyToken.Maui.Views;

public sealed partial class MainPage : ContentPage
{
    public AzureAdConfigurationModel Configuration { get; } = new();

    public MainPage()
    {
        InitializeComponent();

        BindingContext = this;
    }
}
