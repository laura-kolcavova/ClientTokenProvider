using KittyToken.Application.AzureAd.Persistence;
using KittyToken.Application.AzureAd.Services;
using KittyToken.Application.AzureAd.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace KittyToken.Application.AzureAd.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAzureAd(
        this IServiceCollection services)
    {
        services
            .AddScoped(serviceProvider =>
            {
                return new AzureAdFileDbContext(
                    connectionString: "",
                    useDevelopmentLogging: false);
            });

        services
            .AddHttpClient<IAzureAdClient, AzureAdClient>();

        services
            .AddScoped<IAzureAdFileStore, AzureAdFileStoreSqLite>()
            .AddKeyedSingleton<IAzureAdFileStore, AzureAdFileStoreMock>("Mock");

        services
         .AddSingleton<IAzureAdFileFactory, AzureAdFileFactory>();

        return services;
    }
}
