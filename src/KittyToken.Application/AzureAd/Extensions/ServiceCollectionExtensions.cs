using KittyToken.Application.AzureAd.Services;
using KittyToken.Application.AzureAd.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace KittyToken.Application.AzureAd.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAzureAd(
        this IServiceCollection services)
    {
        services
            .AddHttpClient<IAzureAdClient, AzureAdClient>();

        return services;
    }
}