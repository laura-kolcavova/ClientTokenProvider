using KittyToken.Application.Shared.JwtDecoding;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KittyToken.Maui.Models;

public sealed partial class AzureAdConfigurationModel : INotifyPropertyChanged
{
    private string _name = string.Empty;

    private string _instance = string.Empty;

    private string _tenantId = string.Empty;

    private string _clientId = string.Empty;

    private string _clientSecret = string.Empty;

    private string _scope = string.Empty;

    private string? _accessToken = string.Empty;

    private DecodedJwt? _decodedAccessToken;

    private MainPageVisualizationState _visualizationState = MainPageVisualizationState.Idle;

    private (
        string Instance,
        string TenantId,
        string ClientId,
        string ClientSecret,
        string Scope) _savedState;

    public event PropertyChangedEventHandler? PropertyChanged;

    public Guid PersistenceId { get; set; }

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
            OnPropertyChanged(nameof(CanBeSaved));
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
            OnPropertyChanged(nameof(CanBeSaved));
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
            OnPropertyChanged(nameof(CanBeSaved));
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
            OnPropertyChanged(nameof(CanBeSaved));
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
            OnPropertyChanged(nameof(CanBeSaved));
        }
    }

    public string? AccessToken
    {
        get => _accessToken;
        set
        {
            _accessToken = value;
            OnPropertyChanged();
        }
    }

    public DecodedJwt? DecodedAccessToken
    {
        get => _decodedAccessToken;
        set
        {
            _decodedAccessToken = value;
            OnPropertyChanged();
        }
    }

    public MainPageVisualizationState VisualizationState
    {
        get => _visualizationState;
        set
        {
            _visualizationState = value;
            OnPropertyChanged();
        }
    }

    public bool IsPersisted => PersistenceId != Guid.Empty;

    public bool CanBeSaved =>
        !string.IsNullOrWhiteSpace(Name) &&
        (!IsPersisted || _savedState != (Instance, TenantId, ClientId, ClientSecret, Scope));

    public bool CanGetAccessToken =>
        !string.IsNullOrWhiteSpace(Instance) &&
        !string.IsNullOrWhiteSpace(TenantId) &&
        !string.IsNullOrWhiteSpace(ClientId) &&
        !string.IsNullOrWhiteSpace(ClientSecret) &&
        !string.IsNullOrWhiteSpace(Scope);

    public void MarkAsSaved()
    {
        _savedState = (Instance, TenantId, ClientId, ClientSecret, Scope);

        OnPropertyChanged(nameof(CanBeSaved));
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
