using ClientTokenProvider.Business.Shared.Errors;
using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Business.Shared.Serializaiton;
using ClientTokenProvider.Business.Shared.Services.Abstractions;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace ClientTokenProvider.Business.Shared.Services;

internal sealed class ConfigurationExporter(
    IFileSaveHandler fileSaveHandler,
    ILogger<ConfigurationExporter> logger) :
    IConfigurationExporter
{
    public async Task<UnitResult<Error>> Export(
        ConfigurationModel configuration,
        CancellationToken cancellationToken)
    {
        try
        {
            var json = JsonSerializer.Serialize(
                configuration,
                DefaultJsonSerializer.Options);

            var bytes = Encoding.UTF8.GetBytes(json);

            var fileName = !string.IsNullOrEmpty(configuration.Name)
                ? configuration.Name
                : "New Configuration";

            var fileNameWithExtension = $"{fileName}.json";

            using var stream = new MemoryStream(bytes);

            var result = await fileSaveHandler.SaveFile(
                fileNameWithExtension,
                stream,
                cancellationToken);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "An unexpected error occurred while exporting a configuration");

            throw;
        }
    }
}
