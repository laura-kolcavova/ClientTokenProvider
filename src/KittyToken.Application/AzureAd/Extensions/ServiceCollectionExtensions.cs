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
            .AddSingleton<IAzureAdFileFactory, AzureAdFileFactory>();

        services
            .AddSingleton<IAzureAdFileStore, AzureAdFileStoreMock>();

        services
            .AddHttpClient<IAzureAdClient, AzureAdClient>();

        return services;
    }
}
