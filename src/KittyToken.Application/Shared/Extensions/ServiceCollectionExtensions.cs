using KittyToken.Application.AzureAd.Extensions;
using KittyToken.Application.Shared.JwtDecoding;
using KittyToken.Application.Shared.JwtDecoding.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace KittyToken.Application.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services
            .AddSingleton<IJwtDecoder, JwtDecoder>();

        services
            .AddAzureAd();

        return services;
    }
}
