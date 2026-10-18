using ClientTokenProvider.Persistence.Shared.Services.Abstractions;

namespace ClientTokenProvider.Persistence.Shared.Services;

internal sealed class DatabaseInitializer(
    ConfigurationDbContext configurationDbContext) :
    IDatabaseInitializer
{
    public void Initialize()
    {
        configurationDbContext.Init();
    }
}
