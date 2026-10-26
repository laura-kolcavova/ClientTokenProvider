using KittyToken.Application.AzureAd.Dto;
using KittyToken.Application.AzureAd.Models;
using KittyToken.Application.AzureAd.Services.Abstractions;
using KittyToken.Application.Shared.JwtDecoding;
using KittyToken.Application.Shared.JwtDecoding.Abstractions;
using KittyToken.Maui.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace KittyToken.Maui.Views;

public sealed partial class MainPage : ContentPage
{
    private readonly IAzureAdFileStore _azureAdFileStore;

    private readonly IAzureAdClient _azureAdClient;

    private readonly IJwtDecoder _jwtDecoder;


    private string _accessToken = string.Empty;

    private DecodedJwt? _decodedAccessToken;

    private bool _getAccessTokenButtonIsEnabled = false;

    private MainPageVisualizationState _visualizationState = MainPageVisualizationState.Idle;

    private CancellationTokenSource? _getAccessTokenCancellationTokenSource;


    public AzureAdConfigurationModel Configuration { get; } = new();

    public ObservableCollection<AzureAdFile> AzureAdFiles { get; } = [];

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

    public bool GetAccessTokenButtonIsEnabled
    {
        get => _getAccessTokenButtonIsEnabled;
        private set
        {
            _getAccessTokenButtonIsEnabled = value;
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
        IAzureAdClient azureAdClient,
        IJwtDecoder jwtDecoder)
    {
        _azureAdFileStore = azureAdFileStore;
        _azureAdClient = azureAdClient;
        _jwtDecoder = jwtDecoder;

        Configuration.PropertyChanged += Configuration_PropertyChanged;

        InitializeComponent();

        BindingContext = this;

        CheckGetAccessTokenButtonIsEnabled();
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

    private void Configuration_PropertyChanged(
        object? sender,
        PropertyChangedEventArgs e)
    {
        CheckGetAccessTokenButtonIsEnabled();
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

    private void CancelButton_Clicked(
        object? sender,
        EventArgs e)
    {
        _getAccessTokenCancellationTokenSource?.Cancel();
    }

    private async Task GetAccessToken(CancellationToken cancellationToken)
    {
        var request = new GetClientTokenRequestDto
        {
            Instance = Configuration.Instance,
            TenantId = Configuration.TenantId,
            ClientId = Configuration.ClientId,
            ClientSecret = Configuration.ClientSecret,
            Scope = Configuration.Scope,
        };

        AccessToken = await _azureAdClient.GetAcessToken(
            request,
            cancellationToken);

        DecodedAccessToken = _jwtDecoder.Decode(AccessToken);
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

    private void CheckGetAccessTokenButtonIsEnabled()
    {
        var isEnabled = Configuration.IsValid();

        if (GetAccessTokenButtonIsEnabled != isEnabled)
        {
            GetAccessTokenButtonIsEnabled = isEnabled;
        }
    }
}
