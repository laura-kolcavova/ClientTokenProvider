using ClientTokenProvider.Business.Shared.Errors;
using CSharpFunctionalExtensions;

namespace ClientTokenProvider.Business.Shared.Services.Abstractions;

public interface IFilePickHandler
{
    public Task<Result<Stream, Error>> PickFile(
        CancellationToken cancellationToken);
}
