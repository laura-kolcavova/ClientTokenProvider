using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Shared.BindableModels.Abstractions;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientTokenProvider.Shared.BindableModels;

public partial class ConfigurationDetailBindableModel :
    ObservableObject
{
    public Guid Id { get; }

    public ConfigurationKind Kind { get; }

    [ObservableProperty]
    private string _name;

    public IConfigurationDataBindableModel Data { get; }

    [ObservableProperty]
    private bool _canBeSaved;

    [ObservableProperty]
    private bool _canGetAccessToken;

    [ObservableProperty]
    private AccessTokenResult _accessTokenResult;

    [ObservableProperty]
    private bool _isLoading;

    public ConfigurationDetailBindableModel(
        Guid id,
        ConfigurationKind kind,
        string name,
        IConfigurationDataBindableModel data)
    {
        Id = id;
        Kind = kind;
        Name = name;
        Data = data;
        AccessTokenResult = AccessTokenResult.None();
    }
}
