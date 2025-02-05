using ClientTokenProvider.Business.AzureAd.Services;
using ClientTokenProvider.Business.AzureAd.Services.Abstractions;
using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Business.Shared.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace ClientTokenProvider.Business.AzureAd.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientTokenProviderBusinessAzureAd(
        this IServiceCollection services)
    {
        var azureAdConfigurationKindName = Enum.GetName(
          typeof(ConfigurationKind),
          ConfigurationKind.AzureAd)
              ?? throw new InvalidOperationException(
                  $"No name found for enum value '{ConfigurationKind.AzureAd}' of type '{nameof(ConfigurationKind)}'.");

        services
            .AddSingleton<IAzureAdConfigurationFactory, AzureAdConfigurationFactory>();

        services
            .AddKeyedSingleton<IClientTokenProviderFactory, ClientTokenProviderFactory>(azureAdConfigurationKindName);

        return services;
    }
}