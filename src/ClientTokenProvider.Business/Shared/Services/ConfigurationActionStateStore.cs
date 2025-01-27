using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Business.Shared.Services.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace ClientTokenProvider.Business.Shared.Services;

internal sealed class ConfigurationActionStateStore(
    IMemoryCache memoryCache)
    : IConfigurationActionStateStore
{
    private const string CacheKeyFormat = "ConfigurationActionStates_{configurationGuid}";

    public void Remove(ConfigurationModel configuration)
    {
        var key = CreateCacheKey(configuration);

        memoryCache.Remove(key);
    }

    public ConfigurationActionState Get(ConfigurationModel configuration)
    {
        var key = CreateCacheKey(configuration);

        if (memoryCache.TryGetValue<ConfigurationActionState>(
            key,
            out var configurationActionState))
        {
            return configurationActionState;
        }

        return ConfigurationActionState.Idle;
    }

    public void Set(ConfigurationModel configuration, ConfigurationActionState configurationActionState)
    {
        var key = CreateCacheKey(configuration);

        memoryCache.Set(
            key,
            configurationActionState);
    }

    private static string CreateCacheKey(ConfigurationModel configuration)
    {
        return CacheKeyFormat.Replace(
            "{configurationGuid}",
            configuration.Id.ToString());
    }
}
