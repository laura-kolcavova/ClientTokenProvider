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
    private bool isErrorMessageVisible;

    [ObservableProperty]
    private string errorMessage;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(GetClientAccessTokenCommand))]
    private bool getClientAccessTokenButtonEnabled;

    [ObservableProperty]
    private string clientAccessToken;

    public ConfigurationDetailViewModel(
        ILogger<ConfigurationDetailViewModel> logger,
        IAzureAdClientTokenProviderFactory azureAdClientTokenProviderFactory)
    {
        _logger = logger;
        _azureAdClientTokenProviderFactory = azureAdClientTokenProviderFactory;

        configuration = ClientConfigurationModel.Empty;
        errorMessage = string.Empty;
        clientAccessToken = string.Empty;
    }

    partial void OnConfigurationChanged(ClientConfigurationModel value)
    {
        GetClientAccessTokenButtonEnabled = ValidateConfiguration();
    }

    [RelayCommand(
        CanExecute = nameof(GetClientAccessTokenButtonEnabled),
        IncludeCancelCommand = true,
        AllowConcurrentExecutions = false)]
    private async Task GetClientAccessToken(CancellationToken cancellationToken)
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
            var clientAccessToken = await azureAdClientTokenProvider
                .GetAccessToken(Configuration.Scope, cancellationToken);

            HideErrorMessage();

            ClientAccessToken = clientAccessToken;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "An unexpected error occurred while getting client access token");

            ShowErrorMessage(ex.Message);
        }
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

    private void ShowErrorMessage(string message)
    {
        IsErrorMessageVisible = true;
        ErrorMessage = message;
    }

    private void HideErrorMessage()
    {
        IsErrorMessageVisible = false;
        ErrorMessage = string.Empty;
    }
}
