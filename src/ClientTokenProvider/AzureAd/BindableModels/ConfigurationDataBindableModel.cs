using ClientTokenProvider.Business.Shared.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientTokenProvider.AzureAd.BindableViewModels;

public partial class ConfigurationDataBindableModel :
    ObservableObject,
    IConfigurationDataBindableModel
{
    [ObservableProperty]
    private string _authorityUrl = string.Empty;

    [ObservableProperty]
    private string _scope = string.Empty;

    [ObservableProperty]
    private string _audience = string.Empty;

    [ObservableProperty]
    private string _clientId = string.Empty;

    [ObservableProperty]
    private string _clientSecret = string.Empty;

    public ConfigurationDataBindableModel(
        string authorityUrl,
        string scope,
        string audience,
        string clientId,
        string clientSecret)
    {
        AuthorityUrl = authorityUrl;
        Scope = scope;
        Audience = audience;
        ClientId = clientId;
        ClientSecret = clientSecret;
    }
}
