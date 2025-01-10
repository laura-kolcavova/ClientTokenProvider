﻿using ClientTokenProvider.Business.Shared.Factories;
using ClientTokenProvider.Business.Shared.Providers;
using ClientTokenProvider.Business.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ClientTokenProvider.Business.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientTokenProviderBusinessShared(
        this IServiceCollection services)
    {
        services
            .AddMemoryCache();

        services
            .AddSingleton<IConfigurationIdentityProvider, ConfigurationIdentityProvider>()
            .AddSingleton<IConfigurationFactory, ConfigurationFactory>()
            .AddSingleton<IConfigurationCacheService, ConfigurationCacheService>();

        services
            .AddScoped<IConfigurationService, ConfigurationService>();

        return services;
    }
}
