using ClientTokenProvider.Business.Shared.Errors;
using ClientTokenProvider.Business.Shared.Models;
using CSharpFunctionalExtensions;

namespace ClientTokenProvider.Business.Shared.Services.Abstractions;

public interface IFilePickHandler
{
    public Task<Result<Stream, Error>> PickFile(
        FilePickOptions filePickOptions,
        CancellationToken cancellationToken);
}
