using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KittyToken.Maui.Models;

public sealed partial class AzureAdFormDataModel :
    INotifyPropertyChanged
{
    private string _name = string.Empty;

    private string _instance = string.Empty;

    private string _tenantId = string.Empty;

    private string _clientId = string.Empty;

    private string _clientSecret = string.Empty;

    private string _scope = string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    public string Instance
    {
        get => _instance;
        set
        {
            _instance = value;
            OnPropertyChanged();
        }
    }

    public string TenantId
    {
        get => _tenantId;
        set
        {
            _tenantId = value;
            OnPropertyChanged();
        }
    }

    public string ClientId
    {
        get => _clientId;
        set
        {
            _clientId = value;
            OnPropertyChanged();
        }
    }

    public string ClientSecret
    {
        get => _clientSecret;
        set
        {
            _clientSecret = value;
            OnPropertyChanged();
        }
    }

    public string Scope
    {
        get => _scope;
        set
        {
            _scope = value;
            OnPropertyChanged();
        }
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

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
