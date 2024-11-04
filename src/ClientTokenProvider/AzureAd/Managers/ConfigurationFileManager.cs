using ClientTokenProvider.AzureAd.Exceptions;
using ClientTokenProvider.AzureAd.Models;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ClientTokenProvider.AzureAd.Managers;

internal sealed class ConfigurationFileManager(
    ILogger<ConfigurationFileManager> logger) :
    IConfigurationFileManager
{
    public async Task<Result<ClientConfigurationModel, string>> OpenConfiguration(string configurationName, CancellationToken cancellationToken)
    {
        var path = Path.GetFullPath(configurationName);

        try
        {
            var jsonString = await File.ReadAllTextAsync(path, cancellationToken);

            var configuration = JsonSerializer.Deserialize<ClientConfigurationModel>(jsonString) ??
                throw new JsonDeserializationException($"Instance of {nameof(ClientConfigurationModel)} cannot be null");

            return configuration;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "An unexpected error occurred while opening configuration");

            return Result.Failure<ClientConfigurationModel, string>(ex.Message);
        }
    }

    public async Task<UnitResult<string>> SaveConfiguration(
       string configurationName,
       ClientConfigurationModel configuration,
       CancellationToken cancellationToken)
    {
        var path = Path.GetFullPath(configurationName);

        var jsonString = JsonSerializer.Serialize(configuration);

        try
        {
            await File.WriteAllTextAsync(path, jsonString, cancellationToken);

            return UnitResult.Success<string>();
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "An unexpected error occurred while saving configuration");

            return UnitResult.Failure<string>(ex.Message);
        }
    }

    private string GetPath(string configurationName)
    {
        var fileName = $"{configurationName}.json";

        var path = Path.Combine(
            FileSystem.Current.AppDataDirectory,
            fileName);

        return path;
    }

    private string GetFixedPath(string path)
    {
        var exists = File.Exists(path);

        if (!exists)
        {
            return path;
        }

        var fileName = Path.GetFileName(path);
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
        var extension = Path.GetExtension(path);
        var pathWithoutFileName = path.Substring(0, path.Length - fileName.Length);

        var fixedPath = path;
        int fileIndex = 0;

        while (exists)
        {
            fileIndex++;

            fixedPath = Path.Combine(
                pathWithoutFileName,
                $"{fileNameWithoutExtension} ({fileIndex}){extension}");

            exists = File.Exists(fixedPath);
        }

        return fixedPath;
    }
}
