using KittyToken.Application.AzureAd.Extensions;
using KittyToken.Application.Shared.JwtDecoding;
using KittyToken.Application.Shared.JwtDecoding.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace KittyToken.Application.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        string dbConnectionString,
        bool isDevelopment)
    {
        services
            .AddSingleton<IJwtDecoder, JwtDecoder>();

        services
            .AddAzureAd(dbConnectionString, isDevelopment);

        return services;
    }
}
