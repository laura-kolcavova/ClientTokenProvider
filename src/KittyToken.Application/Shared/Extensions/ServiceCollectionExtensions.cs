using KittyToken.Application.AzureAd.Persistence;
using KittyToken.Application.AzureAd.Services;
using KittyToken.Application.AzureAd.Services.Abstractions;
using KittyToken.Application.Shared.JwtDecoding;
using KittyToken.Application.Shared.JwtDecoding.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace KittyToken.Application.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKittyTokenCore(
        this IServiceCollection services)
    {
        services
            .AddSingleton<IJwtDecoder, JwtDecoder>();

        services
            .AddHttpClient<IAzureAdClient, AzureAdClient>();

        return services;
    }

    public static IServiceCollection AddKittyTokenStorage(
        this IServiceCollection services,
        string dbConnectionString,
        bool useDevelopmentLogging)
    {
        services
           .AddScoped(serviceProvider =>
           {
               return new AzureAdFileDbContext(
                   connectionString: dbConnectionString,
                   useDevelopmentLogging: useDevelopmentLogging);
           });

        services
            .AddScoped<IAzureAdFileStore, AzureAdFileStoreSqLite>();

        return services;
    }
}
