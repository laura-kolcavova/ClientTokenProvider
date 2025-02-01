using ClientTokenProvider.Shared.BindableModels;
using CommunityToolkit.Mvvm.Input;

namespace ClientTokenProvider.Shared.Controls;

public partial class ConfigurationDetailPresenter :
    ContentView
{
    public static readonly BindableProperty ConfigurationDetailProperty = BindableProperty.Create(
        nameof(ConfigurationDetail),
        typeof(ConfigurationDetailBindableModel),
        typeof(ConfigurationDetailPresenter));

    public static readonly BindableProperty RenameConfigurationCommandProperty = BindableProperty.Create(
        nameof(RenameConfigurationCommand),
        typeof(IRelayCommand<ConfigurationDetailBindableModel>),
        typeof(ConfigurationDetailPresenter));

    private bool _canRenameConfiguration;

    public ConfigurationDetailBindableModel ConfigurationDetail
    {
        get => (ConfigurationDetailBindableModel)GetValue(ConfigurationDetailProperty);
        set => SetValue(ConfigurationDetailProperty, value);
    }

    public IRelayCommand<ConfigurationDetailBindableModel> RenameConfigurationCommand
    {
        get => (IRelayCommand<ConfigurationDetailBindableModel>)GetValue(RenameConfigurationCommandProperty);
        set => SetValue(RenameConfigurationCommandProperty, value);
    }

    public bool CanStateChange { get; private set; } = true;

    public ConfigurationDetailPresenter()
    {
        InitializeComponent();
    }

    private void ConfigurationNameEntry_Completed(object sender, EventArgs e)
    {
        var entry = (Entry)sender;

        TryRenameConfiguration(entry.Text);
    }

    private void ConfigurationNameEntry_Unfocused(object sender, FocusEventArgs e)
    {
        var entry = (Entry)sender;

        TryRenameConfiguration(entry.Text);

        _canRenameConfiguration = false;
    }

    private void ConfigurationNameEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = (Entry)sender;

        if (entry.IsFocused)
        {
            _canRenameConfiguration = true;
        }
    }

    private void TryRenameConfiguration(string newName)
    {
        if (_canRenameConfiguration)
        {
            ConfigurationDetail.Name = newName;
            RenameConfigurationCommand?.Execute(ConfigurationDetail);
            _canRenameConfiguration = false;
        }
    }
}