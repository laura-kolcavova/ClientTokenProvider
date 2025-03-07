using ClientTokenProvider.Business.Shared.Errors;
using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Business.Shared.Serializaiton;
using ClientTokenProvider.Business.Shared.Services.Abstractions;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ClientTokenProvider.Business.Shared.Services;

internal sealed class ConfigurationImporter(
    IFilePickHandler filePickHandler,
    ConfigurationFileJsonSerializer configurationFileJsonSerializer,
    ILogger<ConfigurationImporter> logger) :
    IConfigurationImporter
{
    public async Task<Result<ConfigurationModel, Error>> Import(
        CancellationToken cancellationToken)
    {
        try
        {
            var filePickOptions = new FilePickOptions
            {
                FileExtensions = [".json"]
            };

            var result = await filePickHandler.PickFile(
                filePickOptions,
                cancellationToken);

            if (result.IsFailure)
            {
                return result.Error;
            }

            using var stream = result.Value;

            var configuration = JsonSerializer.Deserialize<ConfigurationModel>(
                stream,
                configurationFileJsonSerializer.Options);

            if (configuration is null)
            {
                return FileErrors.File.InvalidFormat();
            }

            return configuration;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "An unexpected error occurred while importing a configuration");

            throw;
        }
    }
}
