using ClientTokenProvider.Business.Shared.Errors;
using ClientTokenProvider.Business.Shared.Models;
using CSharpFunctionalExtensions;

namespace ClientTokenProvider.Business.Shared.Services.Abstractions;

public interface IConfigurationExporter
{
    public Task<UnitResult<Error>> Export(
        ConfigurationModel configuration,
        CancellationToken cancellationToken);
}
