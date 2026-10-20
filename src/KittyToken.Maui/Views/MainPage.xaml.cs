using KittyToken.Application.AzureAd.Dto;
using KittyToken.Application.AzureAd.Services.Abstractions;
using KittyToken.Maui.Models;
using System.ComponentModel;

namespace KittyToken.Maui.Views;

public sealed partial class MainPage : ContentPage
{
    private readonly IAzureAdClient _azureAdClient;

    private string _accessToken = string.Empty;

    private bool _getAccessTokenButtonIsEnabled = false;

    private bool _isLoading = false;

    private CancellationTokenSource? _getAccessTokenCancellationTokenSource;

    public AzureAdConfigurationModel Configuration { get; } = new();

    public string AccessToken
    {
        get => _accessToken;
        private set
        {
            _accessToken = value;
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

    public bool IsLoading
    {
        get => _isLoading;
        private set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }

    public MainPage(
        IAzureAdClient azureAdClient)
    {
        _azureAdClient = azureAdClient;

        Configuration.PropertyChanged += Configuration_PropertyChanged;

        InitializeComponent();

        BindingContext = this;

        CheckGetAccessTokenButtonIsEnabled();
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

        IsLoading = true;

        try
        {
            await GetAccessToken(cancellationToken);

            IsLoading = false;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            IsLoading = false;
        }
        catch (Exception ex)
        {
            IsLoading = false;

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
