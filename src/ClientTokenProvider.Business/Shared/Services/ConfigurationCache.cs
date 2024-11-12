using ClientTokenProvider.Business.Shared.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ClientTokenProvider.Business.Shared.Services;

internal sealed class ConfigurationCache(
    IMemoryCache memoryCache) : IConfigurationCache
{
    private const string CacheKey = "SavedConfigurations";

    public void Save(ConfigurationBase configuration)
    {
        if (!memoryCache.TryGetValue<ConfigurationTree>((CacheKey), out var configurationTree)
            || configurationTree is null)
        {
            configurationTree = [];
        }

        var key = configuration.Identity.Id.ToString();

        if (configurationTree.ContainsKey(key))
        {
            configurationTree[key] = configuration;
        }
        else
        {
            configurationTree.Add(key, configuration);
        }

        memoryCache.Set(CacheKey, configurationTree);
    }

    public void Remove(Guid configurationId)
    {
        if (!memoryCache.TryGetValue<ConfigurationTree>((CacheKey), out var configurationTree) ||
            configurationTree is null)
        {
            return;
        }

        var key = configurationId.ToString();

        if (configurationTree.ContainsKey(key))
        {
            return;
        }

        configurationTree.Remove(key);
        memoryCache.Set(CacheKey, configurationTree);
    }

    public IReadOnlyCollection<ConfigurationBase>? GetAll()
    {
        if (!memoryCache.TryGetValue<ConfigurationTree>((CacheKey), out var configurationTree)
            || configurationTree is null)
        {
            return null;
        }

        return configurationTree
            .Values
            .ToList();
    }

    public Configuration<TConfigurationDataType>? Get<TConfigurationDataType>(Guid configurationId) where TConfigurationDataType : notnull
    {
        if (!memoryCache.TryGetValue<ConfigurationTree>((CacheKey), out var configurationTree) ||
            configurationTree is null)
        {
            return null;
        }

        var key = configurationId.ToString();

        if (!configurationTree.TryGetValue(key, out var configuration)
            || configuration is null)
        {
            return null;
        }

        return (Configuration<TConfigurationDataType>)configuration;
    }
}
