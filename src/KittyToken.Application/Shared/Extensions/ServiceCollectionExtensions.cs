using KittyToken.Application.AzureAd.Extensions;
using KittyToken.Application.Shared.JwtDecoding;
using KittyToken.Application.Shared.JwtDecoding.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace KittyToken.Application.Shared.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        IServiceCollection services)
    {
        services
            .AddSingleton<IJwtDecoder, JwtDecoder>();

        services
            .AddAzureAd();

        return services;
    }
}
