using KittyToken.Application.AzureAd.Dto;
using KittyToken.Application.AzureAd.Models;
using KittyToken.Application.AzureAd.Services.Abstractions;
using KittyToken.Application.Shared.JwtDecoding;
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


    private string _accessToken = string.Empty;

    private DecodedJwt? _decodedAccessToken;

    private MainPageVisualizationState _visualizationState = MainPageVisualizationState.Idle;

    private CancellationTokenSource? _getAccessTokenCancellationTokenSource;

    private AzureAdFile? _activeAzureAdFile;


    public ObservableCollection<AzureAdFile> AzureAdFiles { get; } = [];

    public AzureAdFile? ActiveAzureAdFile
    {
        get => _activeAzureAdFile;
        set
        {
            _activeAzureAdFile = value;
            OnPropertyChanged();

            if (value is not null)
            {
                SetFormData(value);
            }
        }
    }

    public AzureAdFormDataModel FormData { get; } = new()
    {
        Name = DefaultAzureAdFileName,
    };

    public string AccessToken
    {
        get => _accessToken;
        private set
        {
            _accessToken = value;
            OnPropertyChanged();
        }
    }

    public DecodedJwt? DecodedAccessToken
    {
        get => _decodedAccessToken;
        private set
        {
            _decodedAccessToken = value;
            OnPropertyChanged();
        }
    }

    public MainPageVisualizationState VisualizationState
    {
        get => _visualizationState;
        private set
        {
            _visualizationState = value;
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

        try
        {
            await LoadAzureAdFiles(CancellationToken.None);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Failed to load Azure AD files",
                ex.Message,
                "OK");
        }
    }

    private async void GetAccessTokenButton_Clicked(
        object? sender,
        EventArgs e)
    {
        using var cancellationTokenSource = new CancellationTokenSource();

        _getAccessTokenCancellationTokenSource = cancellationTokenSource;

        var cancellationToken = cancellationTokenSource.Token;

        AccessToken = string.Empty;
        DecodedAccessToken = null;

        VisualizationState = MainPageVisualizationState.Loading;

        try
        {
            await GetAccessToken(cancellationToken);

            VisualizationState = MainPageVisualizationState.ShowToken;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            VisualizationState = MainPageVisualizationState.Idle;
        }
        catch (Exception ex)
        {
            VisualizationState = MainPageVisualizationState.Idle;

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

    private async void SaveButton_Clicked(
        object? sender,
        EventArgs e)
    {
        try
        {
            await SaveAzureAdFile(CancellationToken.None);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Failed to save Azure AD file",
                ex.Message,
                "OK");
        }
    }

    private async void DeleteMenuFlyoutItem_Clicked(
        object? sender,
        EventArgs e)
    {
        if (sender is not MenuFlyoutItem { BindingContext: AzureAdFile azureAdFile })
        {
            return;
        }

        var confirmed = await DisplayAlertAsync(
            "Delete configuration",
            $"Are you sure you want to delete \"{azureAdFile.Name}\"?",
            "Delete",
            "Cancel");

        if (!confirmed)
        {
            return;
        }

        try
        {
            await DeleteAzureAdFile(azureAdFile, CancellationToken.None);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Failed to delete Azure AD file",
                ex.Message,
                "OK");
        }
    }

    private void CancelButton_Clicked(
        object? sender,
        EventArgs e)
    {
        _getAccessTokenCancellationTokenSource?.Cancel();
    }

    private async Task LoadAzureAdFiles(CancellationToken cancellationToken)
    {
        var azureAdFiles = await _azureAdFileStore.GetAll(cancellationToken);

        AzureAdFiles.Clear();

        foreach (var azureAdFile in azureAdFiles)
        {
            AzureAdFiles.Add(azureAdFile);
        }
    }

    private async Task SaveAzureAdFile(CancellationToken cancellationToken)
    {
        var configuration = new AzureAdConfiguration
        {
            Instance = FormData.Instance,
            TenantId = FormData.TenantId,
            ClientId = FormData.ClientId,
            ClientSecret = FormData.ClientSecret,
            Scope = FormData.Scope,
        };

        if (ActiveAzureAdFile is null)
        {
            var azureAdFile = _azureAdFileFactory.Create(
                FormData.Name,
                configuration);

            await _azureAdFileStore.Add(
                azureAdFile,
                cancellationToken);

            await _azureAdFileStore.SaveChanges(cancellationToken);

            AzureAdFiles.Add(azureAdFile);

            ActiveAzureAdFile = azureAdFile;
        }
        else
        {
            ActiveAzureAdFile.UpdateName(FormData.Name);
            ActiveAzureAdFile.UpdateConfiguration(configuration);

            await _azureAdFileStore.SaveChanges(cancellationToken);
        }
    }

    private async Task DeleteAzureAdFile(
        AzureAdFile azureAdFile,
        CancellationToken cancellationToken)
    {
        _azureAdFileStore.Remove(
            azureAdFile,
            cancellationToken);

        await _azureAdFileStore.SaveChanges(cancellationToken);

        AzureAdFiles.Remove(azureAdFile);

        if (ActiveAzureAdFile == azureAdFile)
        {
            ActiveAzureAdFile = null;

            ResetFormData();
        }

    }

    private async Task GetAccessToken(CancellationToken cancellationToken)
    {
        var request = new GetClientTokenRequestDto
        {
            Instance = FormData.Instance,
            TenantId = FormData.TenantId,
            ClientId = FormData.ClientId,
            ClientSecret = FormData.ClientSecret,
            Scope = FormData.Scope,
        };

        AccessToken = await _azureAdClient.GetAcessToken(
            request,
            cancellationToken);

        DecodedAccessToken = _jwtDecoder.Decode(AccessToken);
    }

    private void ResetFormData()
    {
        FormData.Name = DefaultAzureAdFileName;
        FormData.Instance = string.Empty;
        FormData.TenantId = string.Empty;
        FormData.ClientId = string.Empty;
        FormData.ClientSecret = string.Empty;
        FormData.Scope = string.Empty;
    }

    private void SetFormData(AzureAdFile azureAdFile)
    {
        FormData.Name = azureAdFile.Name;
        FormData.Instance = azureAdFile.Configuration.Instance;
        FormData.TenantId = azureAdFile.Configuration.TenantId;
        FormData.ClientId = azureAdFile.Configuration.ClientId;
        FormData.ClientSecret = azureAdFile.Configuration.ClientSecret;
        FormData.Scope = azureAdFile.Configuration.Scope;
    }
}
