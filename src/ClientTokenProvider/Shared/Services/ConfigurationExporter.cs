using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Business.Shared.Serializaiton;
using ClientTokenProvider.Shared.Services.Abstractions;
using CommunityToolkit.Maui.Storage;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace ClientTokenProvider.Shared.Services;

internal sealed class ConfigurationExporter(
    ILogger<ConfigurationExporter> logger,
    IFileSaver fileSaver) :
    IConfigurationExporter
{
    public async Task Export(
        ConfigurationModel configuration,
        CancellationToken cancellationToken)
    {
        try
        {
            var json = JsonSerializer.Serialize(
                configuration,
                DefaultJsonSerializer.Options);

            var bytes = Encoding.UTF8.GetBytes(json);

            var fileName = $"{configuration.Name}.json";

            using var stream = new MemoryStream(bytes);

#pragma warning disable CA1416 // Validate platform compatibility
            var fileSaverResult = await fileSaver.SaveAsync(
                fileName,
                stream,
                cancellationToken);
#pragma warning restore CA1416 // Validate platform compatibility

            fileSaverResult.EnsureSuccess();
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
