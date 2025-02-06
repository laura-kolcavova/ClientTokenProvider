using ClientTokenProvider.Business.Shared.Models.Abstractions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace ClientTokenProvider.Business.Persistence.Converters;

internal sealed class ConfigurationDataConverter :
    ValueConverter<IConfigurationData, string>
{
    public ConfigurationDataConverter(
        JsonSerializerOptions jsonSerializerOptions)
        : base(
            value => JsonSerializer.Serialize(value, jsonSerializerOptions),
            value => JsonSerializer.Deserialize<IConfigurationData>(value, jsonSerializerOptions)!)
    {
    }
}

