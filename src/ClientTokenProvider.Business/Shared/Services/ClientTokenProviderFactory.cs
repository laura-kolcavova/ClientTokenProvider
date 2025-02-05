using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Business.Shared.Services.Abstractions;
using ClientTokenProvider.Core.AzureAd.Models;
using ClientTokenProvider.Core.Shared.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace ClientTokenProvider.Business.Shared.Services;

internal sealed class ClientTokenProviderFactory(
    IServiceProvider serviceProvider) :
    IClientTokenProviderFactory
{
    public IClientTokenProvider Create(
        ConfigurationKind configurationKind,
        IClientTokenProviderConfiguration clientTokenProviderConfiguration)
    {
        var clientTokenProviderFactory = GetService(configurationKind);

        return clientTokenProviderFactory.Create(
            configurationKind,
            clientTokenProviderConfiguration);
    }

    private IClientTokenProviderFactory GetService(
        ConfigurationKind configurationKind)
    {
        var serviceKey = Enum.GetName(
           typeof(ConfigurationKind),
           configurationKind)
               ?? throw new InvalidOperationException(
                   $"No name found for enum value '{configurationKind}' of type '{nameof(ConfigurationKind)}'.");

        return serviceProvider
            .GetRequiredKeyedService<IClientTokenProviderFactory>(serviceKey);
    }
}
