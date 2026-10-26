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
            OnPropertyChanged(nameof(CanBeSaved));
        }
    }

    public string Instance
    {
        get => _instance;
        set
        {
            _instance = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanGetAccessToken));
        }
    }

    public string TenantId
    {
        get => _tenantId;
        set
        {
            _tenantId = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanGetAccessToken));
        }
    }

    public string ClientId
    {
        get => _clientId;
        set
        {
            _clientId = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanGetAccessToken));
        }
    }

    public string ClientSecret
    {
        get => _clientSecret;
        set
        {
            _clientSecret = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanGetAccessToken));
        }
    }

    public string Scope
    {
        get => _scope;
        set
        {
            _scope = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanGetAccessToken));
        }
    }

    public bool CanBeSaved => !string.IsNullOrWhiteSpace(Name);

    public bool CanGetAccessToken =>
        !string.IsNullOrWhiteSpace(Instance) &&
        !string.IsNullOrWhiteSpace(TenantId) &&
        !string.IsNullOrWhiteSpace(ClientId) &&
        !string.IsNullOrWhiteSpace(ClientSecret) &&
        !string.IsNullOrWhiteSpace(Scope);

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
