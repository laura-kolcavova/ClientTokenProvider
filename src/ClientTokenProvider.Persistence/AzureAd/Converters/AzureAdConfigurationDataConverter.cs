using ClientTokenProvider.Business.AzureAd.Models;
using ClientTokenProvider.Business.Shared.Models.Abstractions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace ClientTokenProvider.Persistence.AzureAd.Converters;

internal class AzureAdConfigurationDataConverter :
    ValueConverter<IConfigurationData, string>
{
    public AzureAdConfigurationDataConverter(
        JsonSerializerOptions? jsonSerializerOptions = null)
        : base(
            value => Serialize(value, jsonSerializerOptions),
            value => JsonSerializer.Deserialize<AzureAdConfigurationData>(value, jsonSerializerOptions)!)
    {
    }

    private static string Serialize(
        IConfigurationData configurationData,
        JsonSerializerOptions? jsonSerializerOptions = null)
    {
        if (configurationData is not AzureAdConfigurationData azureAdConfigurationData)
        {
            throw new ArgumentException("Invalid configuration data type.", nameof(configurationData));
        }

        return JsonSerializer.Serialize(
            azureAdConfigurationData,
            jsonSerializerOptions);
    }
}
