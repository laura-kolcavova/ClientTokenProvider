using ClientTokenProvider.AzureAd.Managers;
using ClientTokenProvider.AzureAd.Mappers;
using ClientTokenProvider.AzureAd.Messages;
using ClientTokenProvider.AzureAd.Models;
using ClientTokenProvider.Core.AzureAd.Factories;
using ClientTokenProvider.Shared.Managers;
using ClientTokenProvider.Shared.Messages;
using ClientTokenProvider.Shared.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;

namespace ClientTokenProvider.AzureAd.ViewModels;

public partial class ConfigurationDetailViewModel : ObservableObject
{
    private readonly IAzureAdClientTokenProviderFactory _azureAdClientTokenProviderFactory;
    private readonly IAzureAdConfigurationManager _azureAdConfigurationFileManager;
    private readonly IConfigurationIdentityManager _configurationIdentityManager;
    private readonly ILogger _logger;

    [ObservableProperty]
    private ConfigurationIdentityModel configurationIdentity;

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

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveConfigurationCommand))]
    private bool saveConfigurationButtonEnabled;

    [ObservableProperty]
    private bool isConfigurationSaved;

    private ActionState _previousState;

    private CancellationTokenSource _cancellationTokenSource;

    public ConfigurationDetailViewModel(
        IAzureAdClientTokenProviderFactory azureAdClientTokenProviderFactory,
        IAzureAdConfigurationManager azureAdConfigurationFileManager,
        IConfigurationIdentityManager configurationIdentityManager,
        ILogger<ConfigurationDetailViewModel> logger)
    {
        _configurationIdentityManager = configurationIdentityManager;
        _azureAdClientTokenProviderFactory = azureAdClientTokenProviderFactory;
        _azureAdConfigurationFileManager = azureAdConfigurationFileManager;
        _logger = logger;

        configurationIdentity = configurationIdentityManager.NewIdentity();
        configuration = ClientConfigurationModel.Empty;
        state = ActionState.Idle;

        errorMessage = string.Empty;
        accessToken = string.Empty;

        _cancellationTokenSource = new CancellationTokenSource();
    }

    partial void OnConfigurationIdentityChanged(ConfigurationIdentityModel value)
    {
        SaveConfigurationButtonEnabled = !Configuration.IsEmpty();
    }

    partial void OnConfigurationChanged(ClientConfigurationModel value)
    {
        GetAccessTokenButtonEnabled = Configuration.IsValid();
        SaveConfigurationButtonEnabled = !Configuration.IsEmpty();
    }

    [RelayCommand]
    private void UpdateAuthority(string authority)
    {
        Configuration = Configuration with
        {
            AuthorityUrl = authority
        };
    }

    [RelayCommand]
    private void UpdateScope(string scope)
    {
        Configuration = Configuration with
        {
            Scope = scope
        };
    }

    [RelayCommand]
    private void UpdateAudience(string audience)
    {
        Configuration = Configuration with
        {
            Audience = audience
        };
    }

    [RelayCommand]
    private void UpdateClientId(string clientId)
    {
        Configuration = Configuration with
        {
            ClientId = clientId
        };
    }

    [RelayCommand]
    private void UpdateClientSecret(string clientSecret)
    {
        Configuration = Configuration with
        {
            ClientSecret = clientSecret
        };
    }

    [RelayCommand(
        CanExecute = nameof(GetAccessTokenButtonEnabled),
        IncludeCancelCommand = true,
        AllowConcurrentExecutions = false)]
    private async Task GetAccessToken(CancellationToken cancellationToken)
    {
        var azureAdClientConfiguration = Configuration.ToClientConfiguration();

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

    [RelayCommand]
    private void ShowErrorDetailModal()
    {
        WeakReferenceMessenger.Default.Send(new ShowErrorDetailMessage
        {
            ErrorMessage = ErrorMessage
        });
    }

    [RelayCommand]
    private void UpdateConfigurationName(string configurationName)
    {
        ConfigurationIdentity = ConfigurationIdentity with
        {
            Name = configurationName
        };

        WeakReferenceMessenger.Default.Send(new ConfigurationNameChangedMessage
        {
            ConfigurationIdentity = ConfigurationIdentity,
            IsConfigurationSaved = IsConfigurationSaved,
        });
    }

    [RelayCommand(
        CanExecute = nameof(SaveConfigurationButtonEnabled))]
    private async Task SaveConfiguration(CancellationToken cancellationToken)
    {
        var result = await _azureAdConfigurationFileManager.SaveConfiguration(
            ConfigurationIdentity,
            Configuration,
            cancellationToken);

        if (result.IsFailure)
        {
            WeakReferenceMessenger.Default.Send(
                new ShowSavingFileFailedErrorMessage());
        }
        else
        {
            SaveConfigurationButtonEnabled = false;
            IsConfigurationSaved = true;

            WeakReferenceMessenger.Default.Send(new ConfigurationSavedMessage
            {
                ConfigurationIdentity = ConfigurationIdentity,
            });
        }
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
