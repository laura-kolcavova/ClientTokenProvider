using ClientTokenProvider.Business.Shared.JsonConverters;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ClientTokenProvider.Business.Shared.Serializaiton;

public static class ConfigurationFileJsonSerializer
{
    public static readonly JsonSerializerOptions Options = new()
    {
        Converters =
        {
            new JsonStringEnumConverter(),
            new ConfigurationFileJsonConverter()
        },

        WriteIndented = true
    };

    public static string Serialize<TValue>(TValue value)
    {
        return JsonSerializer.Serialize(value, Options);
    }

    public static TValue? Deserialize<TValue>(string json)
    {
        return JsonSerializer.Deserialize<TValue>(json, Options);
    }
}
