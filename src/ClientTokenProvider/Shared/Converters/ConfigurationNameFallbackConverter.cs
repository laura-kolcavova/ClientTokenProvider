using CommunityToolkit.Maui.Converters;
using System.Globalization;

namespace ClientTokenProvider.Shared.Converters;

internal sealed class ConfigurationNameFallbackConverter :
     BaseConverterOneWay<string?, string>
{
    public override string DefaultConvertReturnValue { get; set; } = string.Empty;

    public override string ConvertFrom(string? value, CultureInfo? culture)
    {
        // TODO Localization
        return !string.IsNullOrEmpty(value)
            ? value
            : "New Configuration";
    }
}
