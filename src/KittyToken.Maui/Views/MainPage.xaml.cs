using KittyToken.Application.AzureAd.Dto;
using KittyToken.Application.AzureAd.Models;
using KittyToken.Application.AzureAd.Services.Abstractions;
using KittyToken.Application.Shared.JwtDecoding.Abstractions;
using KittyToken.Maui.Models;
using System.Collections.ObjectModel;

namespace KittyToken.Maui.Views;

public sealed partial class MainPage : ContentPage
{
    private const string DefaultAzureAdFileName = "New Configuration";

    private readonly IAzureAdFileStore _azureAdFileStore;

    private readonly IAzureAdFileFactory _azureAdFileFactory;

    private readonly IAzureAdClient _azureAdClient;

    private readonly IJwtDecoder _jwtDecoder;

    private AzureAdConfigurationModel? activeConfiguration;

    private bool _isEditingName;

    private CancellationTokenSource? _getAccessTokenCancellationTokenSource;

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

        await LoadConfigurations();
    }

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
            "Delete configuration",
            $"Are you sure you want to delete \"{configurationModel.Name}\"?",
            "Delete",
            "Cancel");

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
                "Failed to load Azure AD configurations",
                ex.Message,
                "OK");
        }
    }

    private void AddNewConfiguration()
    {
        var configurationModel = new AzureAdConfigurationModel
        {
            Name = DefaultAzureAdFileName
        };

        ConfigurationList.Add(configurationModel);

        ActiveConfiguration = configurationModel;
    }

    private async Task SaveConfiguration(AzureAdConfigurationModel configurationModel)
    {
        if (string.IsNullOrWhiteSpace(configurationModel.Name))
        {
            await DisplayAlertAsync(
               "Failed to save Azure AD configuration",
               "Name must not be empty",
               "OK");

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
                    "Failed to save Azure AD configuration",
                    "Azure AD configuration not found",
                    "OK");

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
                "Failed to save Azure AD configuration",
                ex.Message,
                "OK");
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
                    "Failed to rename Azure AD configuration",
                    "Azure AD configuration not found",
                    "OK");

                return;
            }

            existingAzureAdFile.UpdateName(configurationModel.Name);

            await _azureAdFileStore.SaveChanges(CancellationToken.None);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Failed to rename Azure AD configuration",
                ex.Message,
                "OK");
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
                        "Failed to delete Azure AD configuration",
                        "Azure AD configuration not found",
                        "OK");

                    return;
                }

                _azureAdFileStore.Remove(
                   existingAzureAdFile);

                await _azureAdFileStore.SaveChanges(CancellationToken.None);
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync(
                    "Failed to delete Azure AD configuration",
                    ex.Message,
                    "OK");
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
                "Failed to get access token",
                ex.Message,
                "OK");
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
