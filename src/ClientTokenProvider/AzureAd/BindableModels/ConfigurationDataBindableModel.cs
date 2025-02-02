using ClientTokenProvider.Shared.BindableModels.Abstractions;
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

    public IConfigurationDataBindableModel Copy()
    {
        return new ConfigurationDataBindableModel(
            authorityUrl: AuthorityUrl,
            scope: Scope,
            audience: Audience,
            clientId: ClientId,
            clientSecret: ClientSecret);
    }

    public IEnumerable<object?> GetDataComponents()
    {
        yield return AuthorityUrl;
        yield return Scope;
        yield return Audience;
        yield return ClientId;
        yield return ClientSecret;
    }
}
