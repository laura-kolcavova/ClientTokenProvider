using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Shared.BindableModels.Abstractions;
using ClientTokenProvider.Shared.Services.Abstractions;

namespace ClientTokenProvider.Shared.Services;

internal sealed class ConfigurationDataMapper(IServiceProvider serviceProvider) :
    IConfigurationDataMapper
{
    public IConfigurationDataBindableModel ToBindableModel(IConfigurationData source, ConfigurationKind sourceConfigurationKind)
    {
        var configurationDataMapperService = GetService(sourceConfigurationKind);

        return configurationDataMapperService.ToBindableModel(
            source,
            sourceConfigurationKind);
    }

    public IConfigurationData ToModel(IConfigurationDataBindableModel source, ConfigurationKind sourceConfigurationKind)
    {
        var configurationDataMapperService = GetService(sourceConfigurationKind);

        return configurationDataMapperService.ToModel(
            source,
            sourceConfigurationKind);
    }

    private IConfigurationDataMapper GetService(ConfigurationKind configurationKind)
    {
        var serviceKey = Enum.GetName(
           typeof(ConfigurationKind),
           configurationKind)
               ?? throw new InvalidOperationException(
                   $"No name found for enum value '{configurationKind}' of type '{nameof(ConfigurationKind)}'.");

        return serviceProvider
            .GetRequiredKeyedService<IConfigurationDataMapper>(serviceKey);
    }
}
