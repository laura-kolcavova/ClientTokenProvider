using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Business.Shared.Services.Abstractions;

public interface IConfigurationDataTypeProvider
{
    public Type Get(
        ConfigurationKind kind);
}
