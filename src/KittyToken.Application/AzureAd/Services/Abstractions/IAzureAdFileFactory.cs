using KittyToken.Application.AzureAd.Models;

namespace KittyToken.Application.AzureAd.Services.Abstractions;

public interface IAzureAdFileFactory
{
    public AzureAdFile Create(
        string name,
        AzureAdConfiguration configuration);
}
