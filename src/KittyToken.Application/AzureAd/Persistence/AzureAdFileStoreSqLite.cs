using KittyToken.Application.AzureAd.Models;
using KittyToken.Application.AzureAd.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace KittyToken.Application.AzureAd.Persistence;

internal sealed class AzureAdFileStoreSqLite(
    AzureAdFileDbContext dbContext) :
    IAzureAdFileStore
{
    public async Task<AzureAdFile?> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var azureAdFile = await dbContext
            .AzureAdFiles
            .Where(azureAdFile => azureAdFile.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        return azureAdFile;
    }

    public async Task<IReadOnlyCollection<AzureAdFile>> GetAll(
        CancellationToken cancellationToken)
    {
        var azureAdFiles = await dbContext
            .AzureAdFiles
            .ToListAsync(cancellationToken);

        return azureAdFiles;
    }

    public async Task Add(
        AzureAdFile azureAdFile,
        CancellationToken cancellationToken)
    {
        await dbContext
            .AzureAdFiles
            .AddAsync(
                azureAdFile,
                cancellationToken);
    }

    public void Remove(
        AzureAdFile azureAdFile,
        CancellationToken cancellationToken)
    {
        dbContext
            .AzureAdFiles
            .Remove(azureAdFile);
    }

    public async Task SaveChanges(
        CancellationToken cancellationToken)
    {
        await dbContext.SaveChangesAsync(
           cancellationToken);
    }
}
