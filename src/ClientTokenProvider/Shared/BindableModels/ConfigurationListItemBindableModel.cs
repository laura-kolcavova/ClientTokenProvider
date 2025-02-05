using ClientTokenProvider.Business.Shared.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientTokenProvider.Shared.BindableModels;

public partial class ConfigurationListItemBindableModel :
    ObservableObject
{
    public Guid Id { get; }

    public ConfigurationKind Kind { get; }

    [ObservableProperty]
    private string _name;

    public ConfigurationListItemBindableModel(
        Guid id,
        ConfigurationKind kind,
        string name)
    {
        Id = id;
        Name = name;
        Kind = kind;
    }
}
