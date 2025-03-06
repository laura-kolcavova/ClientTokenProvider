using ClientTokenProvider.Business.Shared.Errors;
using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Business.Shared.Services.Abstractions;
using CSharpFunctionalExtensions;

namespace ClientTokenProvider.Shared.Services;

internal sealed class FilePickHandler(
    IFilePicker filePicker) :
    IFilePickHandler
{
    public async Task<Result<Stream, Error>> PickFile(
        FilePickOptions filePickOptions,
        CancellationToken cancellationToken)
    {
        var targetFileExtensions = filePickOptions
            .FileExtensions
            .ToList();

        var pickOptions = new PickOptions()
        {
            PickerTitle = filePickOptions.Title,
            FileTypes = new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>()
            {
                { DevicePlatform.WinUI, targetFileExtensions }
            })
        };

        var result = await filePicker.PickAsync(
            pickOptions);

        if (result is null)
        {
            return GeneralErrors.General.Cancelled();
        }

        var fileNameParts = result.FileName.Split('.');
        var fileExtension = '.' + fileNameParts[fileNameParts.Length - 1];

        var anyFileExtensionMatch = targetFileExtensions
            .Any(targetFileExtension =>
                string.Equals(
                    targetFileExtension,
                    fileExtension,
                    StringComparison.OrdinalIgnoreCase));

        if (!anyFileExtensionMatch)
        {
            return FileErrors.File.InvalidExtension();
        }

        var stream = await result.OpenReadAsync();

        return stream;
    }
}
