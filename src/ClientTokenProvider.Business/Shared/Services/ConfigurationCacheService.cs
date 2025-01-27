using ClientTokenProvider.Business.Shared.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ClientTokenProvider.Business.Shared.Services;

internal sealed class ConfigurationCacheService(
    IMemoryCache memoryCache) : IConfigurationCacheService
{
    private const string CacheKey = "SavedConfigurations";

    public void Add(ConfigurationModel configuration)
    {
        if (!memoryCache.TryGetValue<ConfigurationTree>(CacheKey, out var configurationTree) ||
            configurationTree is null)
        {
            configurationTree = [];
        }

        configurationTree.Add(configuration.Id, configuration);

        memoryCache.Set(CacheKey, configurationTree);
    }

    public void AddMany(IEnumerable<ConfigurationModel> configurations)
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

    public void Update(ConfigurationModel configuration)
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

    public IReadOnlyCollection<ConfigurationModel>? GetAll()
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

    public ConfigurationModel? Get(Guid configurationId)
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
