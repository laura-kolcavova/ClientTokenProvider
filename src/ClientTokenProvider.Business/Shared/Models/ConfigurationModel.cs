namespace ClientTokenProvider.Business.Shared.Models;

public sealed class ConfigurationModel
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

    public void Rename(string newName)
    {
        Name = newName;
    }
}
