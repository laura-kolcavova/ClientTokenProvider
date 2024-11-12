using ClientTokenProvider.AzureAd.Mappers;
using ClientTokenProvider.AzureAd.Messages;
using ClientTokenProvider.AzureAd.Models;
using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Business.Shared.Providers;
using ClientTokenProvider.Business.Shared.Services;
using ClientTokenProvider.Core.AzureAd.Factories;
using ClientTokenProvider.Shared;
using ClientTokenProvider.Shared.Messages;
using ClientTokenProvider.Shared.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;

namespace ClientTokenProvider.AzureAd.ViewModels;

public partial class ConfigurationDetailViewModel : ObservableObject,
    IQueryAttributable,
    IRecipient<ConfigurationSelectedMessage>
{
    private readonly IAzureAdClientTokenProviderFactory _azureAdClientTokenProviderFactory;
    private readonly IConfigurationRepository _configurationRepository;
    private readonly IConfigurationIdentityProvider _configurationIdentityManager;
    private readonly INavigationService _navigationService;
    private readonly ILogger _logger;

    [ObservableProperty]
    private ConfigurationIdentity configurationIdentity;

    [ObservableProperty]
    private AzureAdConfigurationModel configurationData;

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
        IConfigurationRepository configurationRepository,
        IConfigurationIdentityProvider configurationIdentityManager,
        INavigationService navigationService,
        ILogger<ConfigurationDetailViewModel> logger)
    {
        _configurationIdentityManager = configurationIdentityManager;
        _azureAdClientTokenProviderFactory = azureAdClientTokenProviderFactory;
        _configurationRepository = configurationRepository;
        _navigationService = navigationService;
        _logger = logger;

        configurationIdentity = configurationIdentityManager.NewIdentity();
        configurationData = AzureAdConfigurationModel.Empty;
        state = ActionState.Idle;

        errorMessage = string.Empty;
        accessToken = string.Empty;

        _cancellationTokenSource = new CancellationTokenSource();
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(
            Routes.AzureAdConfigurationDetail.QueryParamConfigurationDetail,
            out var configurationIdObject))
        {
            var configurationId = Guid.Parse(configurationIdObject!.ToString()!);

            try
            {
                _cancellationTokenSource = new CancellationTokenSource();

                var cancellationToken = _cancellationTokenSource.Token;

                await LoadConfiguration(configurationId, cancellationToken);
            }
            finally
            {
                _cancellationTokenSource.Dispose();
            }
        }
    }

    partial void OnConfigurationDataChanged(AzureAdConfigurationModel value)
    {
        GetAccessTokenButtonEnabled = ConfigurationData.IsValid();
        SaveConfigurationButtonEnabled = !ConfigurationData.IsEmpty();
    }

    public async void Receive(ConfigurationSelectedMessage message)
    {
        await _navigationService.GoToAzureAdConfigurationDetail(
            message.ConfigurationIdentity.Id);
    }

    [RelayCommand]
    private void Load()
    {
        WeakReferenceMessenger.Default.RegisterAll(this);
    }

    [RelayCommand]
    private void Unload()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    [RelayCommand]
    private void UpdateAuthority(string authority)
    {
        ConfigurationData = ConfigurationData with
        {
            AuthorityUrl = authority
        };
    }

    [RelayCommand]
    private void UpdateScope(string scope)
    {
        ConfigurationData = ConfigurationData with
        {
            Scope = scope
        };
    }

    [RelayCommand]
    private void UpdateAudience(string audience)
    {
        ConfigurationData = ConfigurationData with
        {
            Audience = audience
        };
    }

    [RelayCommand]
    private void UpdateClientId(string clientId)
    {
        ConfigurationData = ConfigurationData with
        {
            ClientId = clientId
        };
    }

    [RelayCommand]
    private void UpdateClientSecret(string clientSecret)
    {
        ConfigurationData = ConfigurationData with
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
        var azureAdClientConfiguration = ConfigurationData.ToClientConfiguration();

        var azureAdClientTokenProvider = _azureAdClientTokenProviderFactory
            .Create(azureAdClientConfiguration);

        try
        {
            _cancellationTokenSource = new CancellationTokenSource();

            var customCancellationToken = _cancellationTokenSource.Token;

            SwitchToState(ActionState.Loading);

            var clientAccessToken = await azureAdClientTokenProvider
                .GetAccessToken(ConfigurationData.Scope, customCancellationToken);

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

    [RelayCommand(
        IncludeCancelCommand = true,
        AllowConcurrentExecutions = false)]
    private async Task UpdateConfigurationName(
        string configurationName,
        CancellationToken cancellationToken)
    {
        ConfigurationIdentity = ConfigurationIdentity with
        {
            Name = configurationName
        };

        // TODO Add ChangeData method

        if (IsConfigurationSaved)
        {
            // TODO Add ChangeName method
            var configuration = new Configuration<AzureAdConfigurationModel>
            {
                Identity = ConfigurationIdentity,
                Data = ConfigurationData
            };

            await _configurationRepository.Save(
                configuration,
                cancellationToken);

            WeakReferenceMessenger.Default.Send(new ConfigurationNameChangedMessage
            {
                ConfigurationIdentity = ConfigurationIdentity,
                IsConfigurationSaved = IsConfigurationSaved,
            });

            SaveConfigurationButtonEnabled = !ConfigurationData.IsEmpty();
        }
    }

    [RelayCommand(
        CanExecute = nameof(SaveConfigurationButtonEnabled),
        IncludeCancelCommand = true,
        AllowConcurrentExecutions = false)]
    private async Task SaveConfiguration(CancellationToken cancellationToken)
    {
        try
        {
            var configuration = new Configuration<AzureAdConfigurationModel>
            {
                Identity = ConfigurationIdentity,
                Data = ConfigurationData,
            };

            await _configurationRepository.Save(
                configuration,
                cancellationToken);

            SaveConfigurationButtonEnabled = false;
            IsConfigurationSaved = true;

            WeakReferenceMessenger.Default.Send(new ConfigurationSavedMessage
            {
                ConfigurationIdentity = ConfigurationIdentity,
            });
        }
        catch
        {
            WeakReferenceMessenger.Default.Send(
                new ShowSavingFileFailedErrorMessage());
        }
    }

    private async Task LoadConfiguration(
        Guid configurationId,
        CancellationToken cancellationToken)
    {
        var configuration = await _configurationRepository
            .Get<AzureAdConfigurationModel>(
                configurationId,
                cancellationToken);

        if (configuration is null)
        {
            // TODO ERROR
            return;
        }

        ConfigurationIdentity = configuration.Identity;
        ConfigurationData = configuration.Data;
        IsConfigurationSaved = true;
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
