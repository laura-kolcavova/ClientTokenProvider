using ClientTokenProvider.Shared.BindableModels;
using ClientTokenProvider.Shared.Models;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

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

    public static readonly BindableProperty SaveConfigurationDataCommandProperty = BindableProperty.Create(
        nameof(SaveConfigurationDataCommand),
        typeof(IAsyncRelayCommand<ConfigurationDetailBindableModel>),
        typeof(ConfigurationDetailPresenter));

    public static readonly BindableProperty ExportConfigurationCommandProperty = BindableProperty.Create(
       nameof(ExportConfigurationCommand),
       typeof(IAsyncRelayCommand<ConfigurationDetailBindableModel>),
       typeof(ConfigurationDetailPresenter));

    public static readonly BindableProperty HandleConfigurationDataChangedCommandProperty = BindableProperty.Create(
        nameof(HandleConfigurationDataChangedCommand),
        typeof(IRelayCommand<ConfigurationDetailBindableModel>),
        typeof(ConfigurationDetailPresenter));

    public static readonly BindableProperty GetAccessTokenCommandProperty = BindableProperty.Create(
        nameof(GetAccessTokenCommand),
        typeof(IAsyncRelayCommand<ConfigurationDetailBindableModel>),
        typeof(ConfigurationDetailPresenter));

    public static readonly BindableProperty GetAccessTokenCancelCommandProperty = BindableProperty.Create(
        nameof(GetAccessTokenCancelCommand),
        typeof(ICommand),
        typeof(ConfigurationDetailPresenter));

    public static readonly BindableProperty ShowAccessTokenErrorDetailCommandProperty = BindableProperty.Create(
        nameof(ShowAccessTokenErrorDetailCommand),
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

    public IAsyncRelayCommand<ConfigurationDetailBindableModel> SaveConfigurationDataCommand
    {
        get => (IAsyncRelayCommand<ConfigurationDetailBindableModel>)GetValue(SaveConfigurationDataCommandProperty);
        set => SetValue(SaveConfigurationDataCommandProperty, value);
    }

    public IAsyncRelayCommand<ConfigurationDetailBindableModel> ExportConfigurationCommand
    {
        get => (IAsyncRelayCommand<ConfigurationDetailBindableModel>)GetValue(ExportConfigurationCommandProperty);
        set => SetValue(ExportConfigurationCommandProperty, value);
    }

    public IRelayCommand<ConfigurationDetailBindableModel> HandleConfigurationDataChangedCommand
    {
        get => (IRelayCommand<ConfigurationDetailBindableModel>)GetValue(HandleConfigurationDataChangedCommandProperty);
        set => SetValue(HandleConfigurationDataChangedCommandProperty, value);
    }

    public IAsyncRelayCommand<ConfigurationDetailBindableModel> GetAccessTokenCommand
    {
        get => (IAsyncRelayCommand<ConfigurationDetailBindableModel>)GetValue(GetAccessTokenCommandProperty);
        set => SetValue(GetAccessTokenCommandProperty, value);
    }

    public ICommand GetAccessTokenCancelCommand
    {
        get => (ICommand)GetValue(GetAccessTokenCancelCommandProperty);
        set => SetValue(GetAccessTokenCancelCommandProperty, value);
    }

    public IRelayCommand<ConfigurationDetailBindableModel> ShowAccessTokenErrorDetailCommand
    {
        get => (IRelayCommand<ConfigurationDetailBindableModel>)GetValue(ShowAccessTokenErrorDetailCommandProperty);
        set => SetValue(ShowAccessTokenErrorDetailCommandProperty, value);
    }

    public bool CanStateChange { get; private set; } = true;

    public ConfigurationDetailPresenter()
    {
        InitializeComponent();
    }

    [RelayCommand]
    private void HandleConfigurationDataChangedInternal()
    {
        // When ConfigurationDetail is removed this event will be called with null value
        if (ConfigurationDetail is null)
        {
            return;
        }

        HandleConfigurationDataChangedCommand?.Execute(ConfigurationDetail);
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

    private void ExportButton_Clicked(object sender, EventArgs e)
    {
        ExportConfigurationCommand?.Execute(ConfigurationDetail);
    }

    private void SaveConfigurationButton_Clicked(object sender, EventArgs e)
    {
        SaveConfigurationDataCommand?.Execute(ConfigurationDetail);
    }

    private void GetAccessTokenButton_Clicked(object sender, EventArgs e)
    {
        GetAccessTokenCommand?.Execute(ConfigurationDetail);
    }

    private void GetAccessTokenCancelButton_Clicked(object sender, EventArgs e)
    {
        GetAccessTokenCancelCommand?.Execute(null);
    }

    private void AccessTokenErrorLabel_Tapped(object sender, TappedEventArgs e)
    {
        ShowAccessTokenErrorDetailCommand?.Execute(ConfigurationDetail);
    }

    [RelayCommand]
    private void HandleVisualizationModeChanged(JwtVisualizationMode jwtVisualizationMode)
    {
        ConfigurationDetail.AccessTokenVisualizationMode = jwtVisualizationMode;
    }
}
