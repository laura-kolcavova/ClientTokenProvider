using ClientTokenProvider.Business.Shared.Models;
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
    private ConfigurationActionState _actionState;

    [ObservableProperty]
    private bool _canBeSaved;

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
        ActionState = ConfigurationActionState.Idle;
    }
}
