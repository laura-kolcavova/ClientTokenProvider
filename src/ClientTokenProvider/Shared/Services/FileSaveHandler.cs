using ClientTokenProvider.Business.Shared.Errors;
using ClientTokenProvider.Business.Shared.Services.Abstractions;
using CommunityToolkit.Maui.Storage;
using CSharpFunctionalExtensions;

namespace ClientTokenProvider.Shared.Services;

internal sealed class FileSaveHandler(
    IFileSaver fileSaver) :
    IFileSaveHandler
{
    public async Task<UnitResult<Error>> SaveFile(string fileName, Stream stream, CancellationToken cancellationToken)
    {
#pragma warning disable CA1416 // Validate platform compatibility
        var fileSaveResult = await fileSaver.SaveAsync(
            fileName,
            stream,
            cancellationToken);
#pragma warning restore CA1416 // Validate platform compatibility

        if (!fileSaveResult.IsSuccessful)
        {
            if (fileSaveResult.FilePath is null)
            {
                return GeneralErrors.General.Cancelled();
            }

            return FileErrors.File.SavingFailed();
        }

        return UnitResult.Success<Error>();
    }
}
