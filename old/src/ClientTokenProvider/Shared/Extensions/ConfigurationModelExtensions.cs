using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Shared.BindableModels;

namespace ClientTokenProvider.Shared.Extensions;

public static class ConfigurationModelExtensions
{
    public static ConfigurationListItemBindableModel ToListItem(
        this ConfigurationModel source)
    {
        return new ConfigurationListItemBindableModel(
            id: source.Id,
            kind: source.Kind,
            name: source.Name);
    }
}
