using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Shared.Controls;

public partial class ConfigurationDetailPresenter :
    ContentView
{
    public static readonly BindableProperty ConfigurationProperty = BindableProperty.Create(
        nameof(Configuration),
        typeof(ConfigurationModel),
        typeof(ConfigurationDetailPresenter));

    public static readonly BindableProperty ConfigurationActionStateProperty = BindableProperty.Create(
        nameof(Configuration),
        typeof(ConfigurationActionState),
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

    public bool CanStateChange { get; private set; } = true;

    public ConfigurationDetailPresenter()
    {
        InitializeComponent();
    }
}