using ClientTokenProvider.Business.Shared.Errors;
using CSharpFunctionalExtensions;

namespace ClientTokenProvider.Business.Shared.Services.Abstractions;

public interface IFileSaveHandler
{
    public Task<UnitResult<Error>> SaveFile(
        string fileName,
        Stream stream,
        CancellationToken cancellationToken);
}
