//using ClientTokenProvider.AzureAd.Models;
//using ClientTokenProvider.Business.Shared.Exceptions;
//using ClientTokenProvider.Business.Shared.Models;
//using CSharpFunctionalExtensions;
//using Microsoft.Extensions.Logging;
//using System.Text.Json;

//namespace ClientTokenProvider.AzureAd.Managers;

//internal sealed class AzureAdConfigurationManager(
//    ILogger<AzureAdConfigurationManager> logger) :
//    IAzureAdConfigurationManager
//{
//    public async Task<Result<ClientConfigurationModel, string>> OpenConfiguration(
//        ConfigurationIdentity configurationIdentity,
//        CancellationToken cancellationToken)
//    {
//        try
//        {
//            var path = GetPath(configurationIdentity);

//            var jsonString = await File.ReadAllTextAsync(path, cancellationToken);

//            var configuration = JsonSerializer.Deserialize<ClientConfigurationModel>(jsonString) ??
//                throw new JsonDeserializationException($"Instance of {nameof(ClientConfigurationModel)} cannot be null");

//            return configuration;
//        }
//        catch (Exception ex)
//        {
//            logger.LogError(
//                ex,
//                "An unexpected error occurred while opening configuration");

//            return Result.Failure<ClientConfigurationModel, string>(ex.Message);
//        }
//    }

//    public async Task<UnitResult<string>> SaveConfiguration(
//       ConfigurationIdentity configurationIdentity,
//       ClientConfigurationModel configuration,
//       CancellationToken cancellationToken)
//    {
//        try
//        {
//            var path = GetPath(configurationIdentity);

//            var jsonString = JsonSerializer.Serialize(configuration);

//            await File.WriteAllTextAsync(path, jsonString, cancellationToken);

//            return UnitResult.Success<string>();
//        }
//        catch (Exception ex)
//        {
//            logger.LogError(
//                ex,
//                "An unexpected error occurred while saving configuration");

//            return UnitResult.Failure<string>(ex.Message);
//        }
//    }

//    private string GetPath(ConfigurationIdentity configurationIdentity)
//    {
//        var fileName = $"{configurationIdentity.Name}_{configurationIdentity.Id}.json";

//        var path = Path.Combine(
//            FileSystem.Current.AppDataDirectory,
//            fileName);

//        return path;
//    }

//    private string GetFixedPath(string path)
//    {
//        var exists = File.Exists(path);

//        if (!exists)
//        {
//            return path;
//        }

//        var fileName = Path.GetFileName(path);
//        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
//        var extension = Path.GetExtension(path);
//        var pathWithoutFileName = path.Substring(0, path.Length - fileName.Length);

//        var fixedPath = path;
//        int fileIndex = 0;

//        while (exists)
//        {
//            fileIndex++;

//            fixedPath = Path.Combine(
//                pathWithoutFileName,
//                $"{fileNameWithoutExtension} ({fileIndex}){extension}");

//            exists = File.Exists(fixedPath);
//        }

//        return fixedPath;
//    }
//}
