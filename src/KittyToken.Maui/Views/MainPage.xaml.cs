using KittyToken.Application.AzureAd.Dto;
using KittyToken.Application.AzureAd.Models;
using KittyToken.Application.AzureAd.Services.Abstractions;
using KittyToken.Application.Shared.JwtDecoding.Abstractions;
using KittyToken.Maui.Models;
using KittyToken.Maui.Resources.Strings;
using System.Collections.ObjectModel;

namespace KittyToken.Maui.Views;

public sealed partial class MainPage : ContentPage
{
    private readonly IAzureAdFileStore _azureAdFileStore;

    private readonly IAzureAdFileFactory _azureAdFileFactory;

    private readonly IAzureAdClient _azureAdClient;

    private readonly IJwtDecoder _jwtDecoder;

    private AzureAdConfigurationModel? activeConfiguration;

    private bool _isEditingName;

    private CancellationTokenSource? _getAccessTokenCancellationTokenSource;

#if WINDOWS
    private Microsoft.UI.Windowing.AppWindow? _appWindow;

    private bool _canClose;
#endif

    public ObservableCollection<AzureAdConfigurationModel> ConfigurationList { get; } = [];

    public AzureAdConfigurationModel? ActiveConfiguration
    {
        get => activeConfiguration;
        set
        {
            activeConfiguration = value;
            OnPropertyChanged();
        }
    }

    public bool IsEditingName
    {
        get => _isEditingName;
        set
        {
            _isEditingName = value;
            OnPropertyChanged();
        }
    }

    public MainPage(
        IAzureAdFileStore azureAdFileStore,
        IAzureAdFileFactory azureAdFileFactory,
        IAzureAdClient azureAdClient,
        IJwtDecoder jwtDecoder)
    {
        _azureAdFileStore = azureAdFileStore;
        _azureAdFileFactory = azureAdFileFactory;
        _azureAdClient = azureAdClient;
        _jwtDecoder = jwtDecoder;

        InitializeComponent();

        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

#if WINDOWS
        if (_appWindow is null && Window?.Handler?.PlatformView is Microsoft.UI.Xaml.Window platformWindow)
        {
            _appWindow = platformWindow.AppWindow;
            _appWindow.Closing += AppWindow_Closing;
        }
#endif

        await LoadConfigurations();
    }

#if WINDOWS
    private async void AppWindow_Closing(
        Microsoft.UI.Windowing.AppWindow sender,
        Microsoft.UI.Windowing.AppWindowClosingEventArgs args)
    {
        var unsavedConfigurations = ConfigurationList
            .Where(configurationModel => configurationModel.CanBeSaved)
            .ToList();

        if (_canClose || unsavedConfigurations.Count == 0)
        {
            return;
        }

        // Must be set before the first await, otherwise the window closes anyway.
        args.Cancel = true;

        var save = await DisplayAlertAsync(
            MainPageResources.UnsavedChangesTitle,
            MainPageResources.UnsavedChangesMessage,
            MainPageResources.Save,
            MainPageResources.DontSave);

        if (save)
        {
            foreach (var configurationModel in unsavedConfigurations)
            {
                await SaveConfiguration(configurationModel);
            }

            // A failed save already showed its error and stays unsaved; keep the app open so nothing is lost.
            if (ConfigurationList.Any(configurationModel => configurationModel.CanBeSaved))
            {
                return;
            }
        }

        _canClose = true;

        Microsoft.Maui.Controls.Application.Current?.Quit();
    }
#endif

    private void AddConfigurationImageButton_Clicked(
        object? sender,
        EventArgs e)
    {
        AddNewConfiguration();
    }

    private void AddConfigurationButton_Clicked(
        object? sender,
        EventArgs e)
    {
        AddNewConfiguration();
    }

    private async void SaveButton_Clicked(
        object? sender,
        EventArgs e)
    {
        if (ActiveConfiguration is null)
        {
            return;
        }

        await SaveConfiguration(ActiveConfiguration);
    }

    private void NameLabel_Tapped(
        object? sender,
        TappedEventArgs e)
    {
        if (ActiveConfiguration is null)
        {
            return;
        }

        IsEditingName = true;

        NameEntry.Focus();
    }

    private void NameEntry_Completed(
        object? sender,
        EventArgs e)
    {
        NameEntry.Unfocus();
    }

    private async void NameEntry_Unfocused(
        object? sender,
        FocusEventArgs e)
    {
        if (ActiveConfiguration is null)
        {
            return;
        }

        IsEditingName = false;

        if (sender is not Entry entry)
        {
            return;
        }

        var newName = ((Entry)sender).Text;

        await RenameConfiguration(ActiveConfiguration, newName);
    }

    private async void DeleteMenuFlyoutItem_Clicked(
        object? sender,
        EventArgs e)
    {
        if (sender is not MenuFlyoutItem
            {
                BindingContext: AzureAdConfigurationModel configurationModel
            })
        {
            return;
        }

        var confirmed = await DisplayAlertAsync(
            MainPageResources.DeleteConfigurationTitle,
            string.Format(MainPageResources.DeleteConfigurationMessage, configurationModel.Name),
            MainPageResources.Delete,
            MainPageResources.Cancel);

        if (!confirmed)
        {
            return;
        }

        await DeleteConfiguration(configurationModel);
    }

    private async void GetAccessTokenButton_Clicked(
        object? sender,
        EventArgs e)
    {
        if (ActiveConfiguration is null)
        {
            return;
        }

        await GetAccessToken(ActiveConfiguration);
    }

    private void CancelButton_Clicked(
        object? sender,
        EventArgs e)
    {
        _getAccessTokenCancellationTokenSource?.Cancel();
    }

    private async Task LoadConfigurations()
    {
        try
        {
            var azureAdFiles = await _azureAdFileStore.GetAll(CancellationToken.None);

            ConfigurationList.Clear();

            foreach (var azureAdFile in azureAdFiles)
            {
                var configurationModel = CreateConfigurationModel(azureAdFile);

                ConfigurationList.Add(configurationModel);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                MainPageResources.LoadConfigurationsFailed,
                ex.Message,
                MainPageResources.Ok);
        }
    }

    private void AddNewConfiguration()
    {
        var configurationModel = new AzureAdConfigurationModel
        {
            Name = MainPageResources.NewConfigurationName
        };

        ConfigurationList.Add(configurationModel);

        ActiveConfiguration = configurationModel;
    }

    private async Task SaveConfiguration(AzureAdConfigurationModel configurationModel)
    {
        if (string.IsNullOrWhiteSpace(configurationModel.Name))
        {
            await DisplayAlertAsync(
               MainPageResources.SaveConfigurationFailed,
               MainPageResources.NameMustNotBeEmpty,
               MainPageResources.Ok);

            return;
        }

        var configuration = new AzureAdConfiguration
        {
            Instance = configurationModel.Instance,
            TenantId = configurationModel.TenantId,
            ClientId = configurationModel.ClientId,
            ClientSecret = configurationModel.ClientSecret,
            Scope = configurationModel.Scope,
        };

        try
        {
            if (!configurationModel.IsPersisted)
            {
                var newAzureAdFile = _azureAdFileFactory.Create(
                    configurationModel.Name,
                    configuration);

                await _azureAdFileStore.Add(
                     newAzureAdFile,
                     CancellationToken.None);

                await _azureAdFileStore.SaveChanges(CancellationToken.None);

                configurationModel.PersistenceId = newAzureAdFile.Id;

                configurationModel.MarkAsSaved();

                return;
            }

            var existingAzureAdFile = await _azureAdFileStore.GetById(
                configurationModel.PersistenceId,
                CancellationToken.None);

            if (existingAzureAdFile is null)
            {
                await DisplayAlertAsync(
                    MainPageResources.SaveConfigurationFailed,
                    MainPageResources.ConfigurationNotFound,
                    MainPageResources.Ok);

                return;
            }

            existingAzureAdFile.UpdateName(configurationModel.Name);
            existingAzureAdFile.UpdateConfiguration(configuration);

            await _azureAdFileStore.SaveChanges(CancellationToken.None);

            configurationModel.MarkAsSaved();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                MainPageResources.SaveConfigurationFailed,
                ex.Message,
                MainPageResources.Ok);
        }
    }

    private async Task RenameConfiguration(
        AzureAdConfigurationModel configurationModel,
        string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            return;
        }

        configurationModel.Name = newName;

        if (!configurationModel.IsPersisted)
        {
            return;
        }

        try
        {
            var existingAzureAdFile = await _azureAdFileStore.GetById(
                configurationModel.PersistenceId,
                CancellationToken.None);

            if (existingAzureAdFile is null)
            {
                await DisplayAlertAsync(
                    MainPageResources.RenameConfigurationFailed,
                    MainPageResources.ConfigurationNotFound,
                    MainPageResources.Ok);

                return;
            }

            existingAzureAdFile.UpdateName(configurationModel.Name);

            await _azureAdFileStore.SaveChanges(CancellationToken.None);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                MainPageResources.RenameConfigurationFailed,
                ex.Message,
                MainPageResources.Ok);
        }
    }

    private async Task DeleteConfiguration(
        AzureAdConfigurationModel configurationModel)
    {
        if (configurationModel.IsPersisted)
        {
            try
            {
                var existingAzureAdFile = await _azureAdFileStore.GetById(
                    configurationModel.PersistenceId,
                    CancellationToken.None);

                if (existingAzureAdFile is null)
                {
                    await DisplayAlertAsync(
                        MainPageResources.DeleteConfigurationFailed,
                        MainPageResources.ConfigurationNotFound,
                        MainPageResources.Ok);

                    return;
                }

                _azureAdFileStore.Remove(
                   existingAzureAdFile);

                await _azureAdFileStore.SaveChanges(CancellationToken.None);
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync(
                    MainPageResources.DeleteConfigurationFailed,
                    ex.Message,
                    MainPageResources.Ok);
            }
        }

        ConfigurationList.Remove(configurationModel);

        if (ActiveConfiguration == configurationModel)
        {
            ActiveConfiguration = null;
        }
    }

    private async Task GetAccessToken(
        AzureAdConfigurationModel configurationModel)
    {
        using var cancellationTokenSource = new CancellationTokenSource();

        _getAccessTokenCancellationTokenSource = cancellationTokenSource;

        var cancellationToken = cancellationTokenSource.Token;

        configurationModel.AccessToken = null;
        configurationModel.DecodedAccessToken = null;
        configurationModel.VisualizationState = MainPageVisualizationState.Loading;

        var request = new GetClientTokenRequestDto
        {
            Instance = configurationModel.Instance,
            TenantId = configurationModel.TenantId,
            ClientId = configurationModel.ClientId,
            ClientSecret = configurationModel.ClientSecret,
            Scope = configurationModel.Scope,
        };

        try
        {
            var accessToken = await _azureAdClient.GetAcessToken(
                request,
                cancellationToken);

            configurationModel.AccessToken = accessToken;
            configurationModel.DecodedAccessToken = _jwtDecoder.Decode(accessToken);
            configurationModel.VisualizationState = MainPageVisualizationState.ShowToken;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            configurationModel.VisualizationState = MainPageVisualizationState.Idle;
        }
        catch (Exception ex)
        {
            configurationModel.VisualizationState = MainPageVisualizationState.Idle;

            await DisplayAlertAsync(
                MainPageResources.GetAccessTokenFailed,
                ex.Message,
                MainPageResources.Ok);
        }
        finally
        {
            _getAccessTokenCancellationTokenSource = null;
        }
    }

    private static AzureAdConfigurationModel CreateConfigurationModel(AzureAdFile azureAdFile)
    {
        var configurationModel = new AzureAdConfigurationModel
        {
            PersistenceId = azureAdFile.Id,
            Name = azureAdFile.Name,
            Instance = azureAdFile.Configuration.Instance,
            TenantId = azureAdFile.Configuration.TenantId,
            ClientId = azureAdFile.Configuration.ClientId,
            ClientSecret = azureAdFile.Configuration.ClientSecret,
            Scope = azureAdFile.Configuration.Scope,
        };

        configurationModel.MarkAsSaved();

        return configurationModel;
    }
}
