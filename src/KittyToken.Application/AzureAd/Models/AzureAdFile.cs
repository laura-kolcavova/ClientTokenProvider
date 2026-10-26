namespace KittyToken.Application.AzureAd.Models;

public sealed class AzureAdFile
{
    public Guid Id { get; }

    public string Name { get; private set; }

    public AzureAdConfiguration Configuration { get; private set; }

    public AzureAdFile(
        Guid id,
        string name,
        AzureAdConfiguration configuration)
    {
        Id = id;
        Name = name;
        Configuration = configuration;
    }

    public void UpdateName(
        string name)
    {
        Name = name;
    }

    public void UpdateConfiguration(
       AzureAdConfiguration configuration)
    {
        Configuration = configuration;
    }
}
