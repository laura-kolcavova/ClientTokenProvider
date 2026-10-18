using KittyToken.Application.AzureAd.Models;

namespace KittyToken.Application.AzureAd.Services.Abstractions;

public interface IAzureAdFileStore
{
    public Task<AzureAdFile?> GetById(
        Guid id,
        CancellationToken cancellationToken);

    public Task<IReadOnlyCollection<AzureAdFile>> GetAll(
        CancellationToken cancellationToken);

    public Task Add(
        AzureAdFile azureAdFile,
        CancellationToken cancellationToken);

    public Task Remove(
        AzureAdFile azureAdFile,
        CancellationToken cancellationToken);

    public Task SaveChanges(CancellationToken cancellationToken);
}
