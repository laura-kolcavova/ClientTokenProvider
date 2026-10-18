using KittyToken.Application.AzureAd.Models;
using KittyToken.Application.AzureAd.Services.Abstractions;

namespace KittyToken.Application.AzureAd.Persistence;

internal sealed class AzureAdFileStoreMock :
    IAzureAdFileStore
{
    private readonly List<AzureAdFile> _files =
    [
        new AzureAdFile(
            id: Guid.NewGuid(),
            name: "Configuration 1",
            configuration: AzureAdConfiguration.Empty() with
            {
                Instance = "Test Instance 1"
            }),

        new AzureAdFile(
            id: Guid.NewGuid(),
            name: "Configuration 2",
            configuration: AzureAdConfiguration.Empty() with
            {
                Instance = "Test Instance 2"
            }),
    ];

    public Task<AzureAdFile?> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var azureAdFile = _files
            .FirstOrDefault(azureAdFile => azureAdFile.Id == id);

        return Task.FromResult(azureAdFile);
    }

    public Task<IReadOnlyCollection<AzureAdFile>> GetAll(
        CancellationToken cancellationToken)
    {
        var azureAdFiles = _files.ToList();

        return Task.FromResult<IReadOnlyCollection<AzureAdFile>>(azureAdFiles);
    }

    public Task Add(
        AzureAdFile azureAdFile,
        CancellationToken cancellationToken)
    {
        _files.Add(azureAdFile);

        return Task.CompletedTask;
    }

    public void Remove(
        AzureAdFile azureAdFile,
        CancellationToken cancellationToken)
    {
        _files.Remove(azureAdFile);
    }

    public Task SaveChanges(
        CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
