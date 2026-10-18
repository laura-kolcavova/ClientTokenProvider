using CommunityToolkit.Maui.Converters;
using System.Globalization;
using System.Text.Json;

namespace ClientTokenProvider.Shared.Converters;

internal sealed class JsonPrettyConverter :
     BaseConverterOneWay<string, string>
{
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
                new JsonSerializerOptions
                {
                    WriteIndented = true,
                });

            return formattedJson;
        }
        catch
        {
            return value;
        }
    }
}
