using CommunityToolkit.Maui.Converters;
using System.Globalization;

namespace ClientTokenProvider.Shared.Converters;

internal sealed class ConfigurationNameFallbackConverter :
     BaseConverterOneWay<string, string>
{
    public override string DefaultConvertReturnValue { get; set; } = "New Configuration";

    public override string ConvertFrom(string value, CultureInfo? culture)
    {
        var isNameSet = !string.IsNullOrEmpty(value);

        // TODO Localization
        return isNameSet
            ? value
            : DefaultConvertReturnValue;
    }
}
