using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Business.Shared.Models.Abstractions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ClientTokenProvider.Business.Shared.JsonConverters;

internal sealed class ConfigurationFileJsonConverter :
    JsonConverter<ConfigurationModel>
{
    public override ConfigurationModel? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        string? name = null;
        ConfigurationKind? kind = null;
        IConfigurationData? data = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();

                reader.Read();

                switch (propertyName)
                {
                    case nameof(ConfigurationModel.Name):
                        name = reader.GetString();

                        break;
                    case nameof(ConfigurationModel.Kind):
                        kind = Enum.Parse<ConfigurationKind>(
                            reader.GetString() ?? string.Empty);

                        break;
                    case nameof(ConfigurationModel.Data):
                        data = JsonSerializer.Deserialize<IConfigurationData>(
                            ref reader,
                            options);

                        break;
                }
            }
        }

        var configuration = new ConfigurationModel(
            id: Guid.NewGuid(),
            name: name
                ?? throw new JsonException("Failed to deserialize Name property"),
            kind: kind
                ?? throw new JsonException("Failed to deserialize Kind property"),
            data: data ??
                throw new JsonException("Failed to deserialize Data property"));

        return configuration;
    }

    public override void Write(
        Utf8JsonWriter writer,
        ConfigurationModel value,
        JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString(
            nameof(ConfigurationModel.Name),
            value.Name);

        writer.WriteString(
            nameof(ConfigurationModel.Kind),
            Enum.GetName<ConfigurationKind>(value.Kind));

        writer.WritePropertyName(
            nameof(ConfigurationModel.Data));

        JsonSerializer.Serialize(
            writer,
            value.Data,
            options);

        writer.WriteEndObject();
    }
}
