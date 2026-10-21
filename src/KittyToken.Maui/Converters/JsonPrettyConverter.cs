using CommunityToolkit.Maui.Converters;
using System.Globalization;
using System.Text.Json;

namespace KittyToken.Maui.Converters;

internal sealed partial class JsonPrettyConverter :
     BaseConverterOneWay<string, string>
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        WriteIndented = true,
    };

    public override string DefaultConvertReturnValue { get; set; } = string.Empty;

    public override string ConvertFrom(string value, CultureInfo? culture)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        try
        {
            using var jsonDocument = JsonDocument.Parse(value);

            var formattedJson = JsonSerializer.Serialize(
                jsonDocument.RootElement,
                _jsonSerializerOptions
              );

            return formattedJson;
        }
        catch
        {
            return value;
        }
    }
}
