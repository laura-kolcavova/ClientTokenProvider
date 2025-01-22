using CommunityToolkit.Maui.Converters;
using System.Globalization;

namespace ClientTokenProvider.Shared.Converters;

internal sealed class ConfigurationNameFallbackConverter :
     BaseConverterOneWay<string?, object>
{
    public override object DefaultConvertReturnValue { get; set; } = string.Empty;

    public override object ConvertFrom(string? value, CultureInfo? culture)
    {
        return !string.IsNullOrEmpty(value)
            ? value
            : "New Configuration";
    }
}
