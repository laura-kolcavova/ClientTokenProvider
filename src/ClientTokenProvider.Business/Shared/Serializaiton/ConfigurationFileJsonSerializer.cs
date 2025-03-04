using ClientTokenProvider.Business.Shared.JsonConverters;
using ClientTokenProvider.Business.Shared.Services.Abstractions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ClientTokenProvider.Business.Shared.Serializaiton;

public sealed class ConfigurationFileJsonSerializer(
    IConfigurationDataTypeProvider configurationDataTypeProvider)
{
    public JsonSerializerOptions Options { get; } = new()
    {
        Converters =
        {
            new JsonStringEnumConverter(),
            new ConfigurationFileJsonConverter(configurationDataTypeProvider)
        },

        WriteIndented = true
    };

    public string Serialize<TValue>(TValue value)
    {
        return JsonSerializer.Serialize(value, Options);
    }

    public TValue? Deserialize<TValue>(string json)
    {
        return JsonSerializer.Deserialize<TValue>(json, Options);
    }
}
