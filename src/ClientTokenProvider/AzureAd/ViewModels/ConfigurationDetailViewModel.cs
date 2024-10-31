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
    private ClientConfigurationModel _configuration;

    [ObservableProperty]
    private bool _isErrorMessageVisible;

    [ObservableProperty]
    private string _errorMessage;

    public ConfigurationDetailViewModel(
        ILogger<ConfigurationDetailViewModel> logger,
        IAzureAdClientTokenProviderFactory azureAdClientTokenProviderFactory)
    {
        _logger = logger;
        _azureAdClientTokenProviderFactory = azureAdClientTokenProviderFactory;

        _configuration = ClientConfigurationModel.Empty;
        _errorMessage = string.Empty;
    }

    [RelayCommand]
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
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "An unexpected error occurred while getting client access token");

            ShowErrorMessage(ex.Message);
        }
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
