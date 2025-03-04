using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Business.Shared.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace ClientTokenProvider.Business.Shared.Services;

internal sealed class ConfigurationDataTypeProvider(
    IServiceProvider serviceProvider) :
    IConfigurationDataTypeProvider
{
    public Type Get(ConfigurationKind kind)
    {
        var configurationDataTypeProvider = GetService(kind);

        return configurationDataTypeProvider.Get(kind);
    }

    private IConfigurationDataTypeProvider GetService(
       ConfigurationKind configurationKind)
    {
        var serviceKey = Enum.GetName(
           typeof(ConfigurationKind),
           configurationKind)
               ?? throw new InvalidOperationException(
                   $"No name found for enum value '{configurationKind}' of type '{nameof(ConfigurationKind)}'.");

        return serviceProvider
            .GetRequiredKeyedService<IConfigurationDataTypeProvider>(serviceKey);
    }
}
