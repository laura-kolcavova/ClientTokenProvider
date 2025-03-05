using ClientTokenProvider.Business.Shared.Errors;
using ClientTokenProvider.Business.Shared.Models;
using CSharpFunctionalExtensions;

namespace ClientTokenProvider.Business.Shared.Services.Abstractions;

public interface IConfigurationImporter
{
    public Task<Result<ConfigurationModel, Error>> Import(
        CancellationToken cancellationToken);
}
