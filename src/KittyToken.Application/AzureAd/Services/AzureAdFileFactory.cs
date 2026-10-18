using KittyToken.Application.AzureAd.Models;
using KittyToken.Application.AzureAd.Services.Abstractions;

namespace KittyToken.Application.AzureAd.Services;

internal sealed class AzureAdFileFactory :
    IAzureAdFileFactory
{
    public AzureAdFile Create(
        string name,
        AzureAdConfiguration configuration)
    {
        var id = Guid.NewGuid();

        var azureAdFile = new AzureAdFile(
            id,
            name,
            configuration);

        return azureAdFile;
    }
}
