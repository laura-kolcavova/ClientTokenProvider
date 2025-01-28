using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Shared.Models;
using CommunityToolkit.Mvvm.Input;

namespace ClientTokenProvider.Shared.Controls;

public partial class ConfigurationDetailPresenter :
    ContentView
{
    public static readonly BindableProperty ConfigurationProperty = BindableProperty.Create(
        nameof(Configuration),
        typeof(ConfigurationModel),
        typeof(ConfigurationDetailPresenter));

    public static readonly BindableProperty ConfigurationActionStateProperty = BindableProperty.Create(
        nameof(ConfigurationActionState),
        typeof(ConfigurationActionState),
        typeof(ConfigurationDetailPresenter));

    public static readonly BindableProperty RenameConfigurationCommandProperty = BindableProperty.Create(
        nameof(RenameConfigurationCommand),
        typeof(IRelayCommand<RenameConfigurationRequest>),
        typeof(ConfigurationDetailPresenter));

    public ConfigurationModel Configuration
    {
        get => (ConfigurationModel)GetValue(ConfigurationProperty);
        set => SetValue(ConfigurationProperty, value);
    }

    public ConfigurationActionState ConfigurationActionState
    {
        get => (ConfigurationActionState)GetValue(ConfigurationActionStateProperty);
        set => SetValue(ConfigurationActionStateProperty, value);
    }

    public IRelayCommand<RenameConfigurationRequest> RenameConfigurationCommand
    {
        get => (IRelayCommand<RenameConfigurationRequest>)GetValue(RenameConfigurationCommandProperty);
        set => SetValue(RenameConfigurationCommandProperty, value);
    }

    public bool CanStateChange { get; private set; } = true;

    public ConfigurationDetailPresenter()
    {
        InitializeComponent();
    }

    private void ConfigurationNameEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        var request = new RenameConfigurationRequest
        {
            Configuration = Configuration,
            NewName = e.NewTextValue
        };

        RenameConfigurationCommand?.Execute(request);
    }
}