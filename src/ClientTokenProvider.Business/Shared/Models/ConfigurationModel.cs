using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClientTokenProvider.Business.Shared.Models;

public class ConfigurationModel : INotifyPropertyChanged
{
    public Guid Id { get; }

    public string Name { get; private set; }

    public ConfigurationKind Kind { get; }

    public IConfigurationData Data { get; }

    public ConfigurationModel(Guid id, string name, ConfigurationKind kind, IConfigurationData data)
    {
        Id = id;
        Name = name;
        Kind = kind;
        Data = data;
    }

    public ConfigurationModel(Guid id, ConfigurationKind kind, IConfigurationData data) :
        this(id, string.Empty, kind, data)
    {
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void Rename(string newName)
    {
        Name = newName;
        OnPropertyChanged(nameof(Name));
    }

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
