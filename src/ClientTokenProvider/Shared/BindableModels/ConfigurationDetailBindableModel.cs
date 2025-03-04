using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Shared.BindableModels.Abstractions;
using ClientTokenProvider.Shared.Models;
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
    private bool _canBeExported;

    [ObservableProperty]
    private bool _canGetAccessToken;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private AccessTokenResult _accessTokenResult;

    [ObservableProperty]
    private JwtVisualizationMode _accessTokenVisualizationMode;

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

        AccessTokenResult = Models.AccessTokenResult.None();
        AccessTokenVisualizationMode = JwtVisualizationMode.JwtDecoded;
    }
}
