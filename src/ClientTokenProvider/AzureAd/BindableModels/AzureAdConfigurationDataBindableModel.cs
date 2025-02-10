using ClientTokenProvider.Shared.BindableModels.Abstractions;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientTokenProvider.AzureAd.BindableViewModels;

public partial class AzureAdConfigurationDataBindableModel :
    ObservableObject,
    IConfigurationDataBindableModel
{
    [ObservableProperty]
    private string _instance = string.Empty;

    [ObservableProperty]
    private string _tenantId = string.Empty;

    [ObservableProperty]
    private string _scope = string.Empty;

    [ObservableProperty]
    private string _audience = string.Empty;

    [ObservableProperty]
    private string _clientId = string.Empty;

    [ObservableProperty]
    private string _clientSecret = string.Empty;

    public AzureAdConfigurationDataBindableModel(
        string instance,
        string tenantId,
        string scope,
        string audience,
        string clientId,
        string clientSecret)
    {
        Instance = instance;
        TenantId = tenantId;
        Scope = scope;
        Audience = audience;
        ClientId = clientId;
        ClientSecret = clientSecret;
    }

    public bool AreDataValid()
    {
        return
            !string.IsNullOrEmpty(Instance) &&
            !string.IsNullOrEmpty(TenantId) &&
            !string.IsNullOrEmpty(Scope) &&
            !string.IsNullOrEmpty(Audience) &&
            !string.IsNullOrEmpty(ClientId) &&
            !string.IsNullOrEmpty(ClientSecret);
    }

    public IConfigurationDataBindableModel Copy()
    {
        return new AzureAdConfigurationDataBindableModel(
            instance: Instance,
            tenantId: TenantId,
            scope: Scope,
            audience: Audience,
            clientId: ClientId,
            clientSecret: ClientSecret);
    }

    public IEnumerable<object?> GetDataComponents()
    {
        yield return Instance;
        yield return TenantId;
        yield return Scope;
        yield return Audience;
        yield return ClientId;
        yield return ClientSecret;
    }
}
