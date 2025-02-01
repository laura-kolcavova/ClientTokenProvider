using ClientTokenProvider.Business.AzureAd.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientTokenProvider.AzureAd.BindableViewModels;

public partial class ConfigurationDataBindableModel :
    ObservableObject
{
    private readonly AzureAdConfigurationData _configurationData;

    [ObservableProperty]
    public string _authorityUrl = string.Empty;

    [ObservableProperty]
    public string _scope = string.Empty;

    [ObservableProperty]
    public string _audience = string.Empty;

    [ObservableProperty]
    public string _clientId = string.Empty;

    [ObservableProperty]
    public string _clientSecret = string.Empty;

    public ConfigurationDataBindableModel(
        AzureAdConfigurationData configurationData)
    {
        _configurationData = configurationData;

        SetProperties();
    }

    public void Refresh()
    {
        SetProperties();
    }

    private void SetProperties()
    {
        AuthorityUrl = _configurationData.AuthorityUrl;
        Scope = _configurationData.Scope;
        Audience = _configurationData.Audience;
        ClientId = _configurationData.ClientId;
        ClientSecret = _configurationData.ClientSecret;
    }
}
