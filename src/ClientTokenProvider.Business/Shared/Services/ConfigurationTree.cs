using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Business.Shared.Services;

internal sealed class ConfigurationTree :
    Dictionary<Guid, ConfigurationModel>;
