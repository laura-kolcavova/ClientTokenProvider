using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Business.Shared.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ClientTokenProvider.Business.Persistence.Shared.Services;

internal sealed class ConfigurationRepositorySqlLite(
    ConfigurationDbContext configurationDbContext) :
    IConfigurationRepository
{
    public async Task Add(
        ConfigurationModel configuration,
        CancellationToken cancellationToken)
    {
        await configurationDbContext
            .Configurations
            .AddAsync(
                configuration,
                cancellationToken);

        await configurationDbContext.SaveChangesAsync(
            cancellationToken);
    }

    public async Task Delete(
        ConfigurationModel configuration,
        CancellationToken cancellationToken)
    {
        configurationDbContext
           .Configurations
           .Remove(configuration);

        await configurationDbContext.SaveChangesAsync(
            cancellationToken);
    }

    public async Task<ConfigurationModel?> Get(
        Guid configurationId,
        CancellationToken cancellationToken)
    {
        return await configurationDbContext
            .Configurations
            .Where(configuration => configuration.Id == configurationId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<ConfigurationModel>> GetAll(
        CancellationToken cancellationToken)
    {
        return await configurationDbContext
            .Configurations
            .ToListAsync(cancellationToken);
    }

    public async Task Update(
        ConfigurationModel configuration,
        CancellationToken cancellationToken)
    {
        configurationDbContext
            .Configurations
            .Update(configuration);

        await configurationDbContext
            .SaveChangesAsync(cancellationToken);
    }
}
