using ClientTokenProvider.Business.Shared.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ClientTokenProvider.Business.Shared.Services;

internal sealed class ConfigurationCacheService(
    IMemoryCache memoryCache) : IConfigurationCacheService
{
    private const string CacheKey = "SavedConfigurations";

    public void Add(Configuration configuration)
    {
        if (!memoryCache.TryGetValue<ConfigurationTree>(CacheKey, out var configurationTree) ||
            configurationTree is null)
        {
            configurationTree = [];
        }

        configurationTree.Add(configuration.Id, configuration);

        memoryCache.Set(CacheKey, configurationTree);
    }

    public void AddMany(IEnumerable<Configuration> configurations)
    {
        if (!memoryCache.TryGetValue<ConfigurationTree>(CacheKey, out var configurationTree) ||
            configurationTree is null)
        {
            configurationTree = [];
        }

        foreach (var configuration in configurations)
        {
            configurationTree.Add(configuration.Id, configuration);
        }

        memoryCache.Set(CacheKey, configurationTree);
    }

    public void Update(Configuration configuration)
    {
        if (!memoryCache.TryGetValue<ConfigurationTree>(CacheKey, out var configurationTree)
            || configurationTree is null)
        {
            configurationTree = [];
        }

        var configurationId = configuration.Id;

        if (configurationTree.ContainsKey(configurationId))
        {
            configurationTree[configurationId] = configuration;
        }
        else
        {
            configurationTree.Add(configurationId, configuration);
        }

        memoryCache.Set(CacheKey, configurationTree);
    }

    public void Remove(Guid configurationId)
    {
        if (!memoryCache.TryGetValue<ConfigurationTree>(CacheKey, out var configurationTree) ||
            configurationTree is null)
        {
            return;
        }

        if (configurationTree.ContainsKey(configurationId))
        {
            return;
        }

        configurationTree.Remove(configurationId);

        memoryCache.Set(CacheKey, configurationTree);
    }

    public IReadOnlyCollection<Configuration>? GetAll()
    {
        if (!memoryCache.TryGetValue<ConfigurationTree>(CacheKey, out var configurationTree)
            || configurationTree is null)
        {
            return null;
        }

        return configurationTree
            .Values
            .ToList();
    }

    public Configuration? Get(Guid configurationId)
    {
        if (!memoryCache.TryGetValue<ConfigurationTree>(CacheKey, out var configurationTree) ||
            configurationTree is null)
        {
            return null;
        }

        if (!configurationTree.TryGetValue(configurationId, out var configuration)
            || configuration is null)
        {
            return null;
        }

        return configuration;
    }
}
