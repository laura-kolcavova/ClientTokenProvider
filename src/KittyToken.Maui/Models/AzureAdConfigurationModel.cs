using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KittyToken.Maui.Models;

public sealed partial class AzureAdConfigurationModel :
    INotifyPropertyChanged
{
    private string _instance = string.Empty;

    private string _tenantId = string.Empty;

    private string _clientId = string.Empty;

    private string _clientSecret = string.Empty;

    private string _scope = string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Instance
    {
        get => _instance;
        set => SetProperty(ref _instance, value);
    }

    public string TenantId
    {
        get => _tenantId;
        set => SetProperty(ref _tenantId, value);
    }

    public string ClientId
    {
        get => _clientId;
        set => SetProperty(ref _clientId, value);
    }

    public string ClientSecret
    {
        get => _clientSecret;
        set => SetProperty(ref _clientSecret, value);
    }

    public string Scope
    {
        get => _scope;
        set => SetProperty(ref _scope, value);
    }

    public bool IsValid()
    {
        return
            !string.IsNullOrEmpty(Instance) &&
            !string.IsNullOrEmpty(TenantId) &&
            !string.IsNullOrEmpty(ClientId) &&
            !string.IsNullOrEmpty(ClientSecret) &&
            !string.IsNullOrEmpty(Scope);
    }

    private void SetProperty(
        ref string field,
        string value,
        [CallerMemberName] string? propertyName = null)
    {
        if (field == value)
        {
            return;
        }

        field = value;

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
