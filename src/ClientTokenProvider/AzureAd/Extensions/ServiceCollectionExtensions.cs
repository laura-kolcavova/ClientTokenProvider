using ClientTokenProvider.AzureAd.Services;
using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Shared.Services.Abstractions;

namespace ClientTokenProvider.AzureAd.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientTokenProviderAzureAd(
        this IServiceCollection services)
    {
        var azureAdConfigurationKindName = Enum.GetName(
           typeof(ConfigurationKind),
           ConfigurationKind.AzureAd)
               ?? throw new InvalidOperationException(
                   $"No name found for enum value '{ConfigurationKind.AzureAd}' of type '{nameof(ConfigurationKind)}'.");
        services
            .AddKeyedSingleton<IConfigurationDataMapper, ConfigurationDataMapper>(azureAdConfigurationKindName);

        return services;
    }
}
