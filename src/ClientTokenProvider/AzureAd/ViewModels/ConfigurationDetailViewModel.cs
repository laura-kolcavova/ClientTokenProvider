using ClientTokenProvider.AzureAd.Models;
using ClientTokenProvider.Core.AzureAd.Factories;
using ClientTokenProvider.Core.AzureAd.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace ClientTokenProvider.AzureAd.ViewModels;

public partial class ConfigurationDetailViewModel : ObservableObject
{
    private readonly ILogger _logger;
    private readonly IAzureAdClientTokenProviderFactory _azureAdClientTokenProviderFactory;

    [ObservableProperty]
    private ClientConfigurationModel configuration;

    [ObservableProperty]
    private ActionState state;

    [ObservableProperty]
    private string errorMessage;

    [ObservableProperty]
    private string accessToken;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(GetAccessTokenCommand))]
    private bool getAccessTokenButtonEnabled;

    private ActionState _previousState;

    private CancellationTokenSource _cancellationTokenSource;

    public ConfigurationDetailViewModel(
        ILogger<ConfigurationDetailViewModel> logger,
        IAzureAdClientTokenProviderFactory azureAdClientTokenProviderFactory)
    {
        _logger = logger;
        _azureAdClientTokenProviderFactory = azureAdClientTokenProviderFactory;

        configuration = ClientConfigurationModel.Empty;
        state = ActionState.Loading;
        errorMessage = string.Empty;
        accessToken = string.Empty;
        _cancellationTokenSource = new CancellationTokenSource();
    }

    partial void OnConfigurationChanged(ClientConfigurationModel value)
    {
        GetAccessTokenButtonEnabled = ValidateConfiguration();
    }

    [RelayCommand(
        CanExecute = nameof(GetAccessTokenButtonEnabled),
        IncludeCancelCommand = true,
        AllowConcurrentExecutions = false)]
    private async Task GetAccessToken(CancellationToken cancellationToken)
    {
        var azureAdClientConfiguration = new ClientConfiguration
        {
            Audience = Configuration.Audience,
            AuthorityUri = Configuration.AuthorityUrl,
            ClientId = Configuration.ClientId,
            ClientSecret = Configuration.ClientSecret,
        };

        var azureAdClientTokenProvider = _azureAdClientTokenProviderFactory
            .Create(azureAdClientConfiguration);

        try
        {
            _cancellationTokenSource = new CancellationTokenSource();

            var customCancellationToken = _cancellationTokenSource.Token;

            SwitchToState(ActionState.Loading);

            var clientAccessToken = await azureAdClientTokenProvider
                .GetAccessToken(Configuration.Scope, customCancellationToken);

            HandleSuccess(clientAccessToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "An unexpected error occurred while getting client access token");

            HandleError(ex.Message);
        }
        finally
        {
            _cancellationTokenSource.Dispose();
        }
    }

    [RelayCommand]
    private void CancelRequest()
    {
        SwitchToState(_previousState);

        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }

    private bool ValidateConfiguration()
    {
        if (string.IsNullOrEmpty(Configuration.Audience) ||
            string.IsNullOrEmpty(Configuration.AuthorityUrl) ||
            string.IsNullOrEmpty(Configuration.ClientId) ||
            string.IsNullOrEmpty(Configuration.ClientSecret) ||
            string.IsNullOrEmpty(Configuration.Scope))
        {
            return false;
        }

        return true;
    }

    private void SwitchToState(ActionState state)
    {
        _previousState = State;
        State = state;
    }

    private void SwitchToPreviousState()
    {
        SwitchToState(_previousState);
    }

    private void HandleError(string message)
    {

        ErrorMessage = message;
        SwitchToState(ActionState.Error);
    }

    private void HandleSuccess(string accessToken)
    {
        AccessToken = accessToken;
        SwitchToState(ActionState.Success);
    }
}
