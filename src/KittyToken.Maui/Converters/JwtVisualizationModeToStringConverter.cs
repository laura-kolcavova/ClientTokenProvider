using CommunityToolkit.Maui.Converters;
using KittyToken.Maui.Models;
using System.Globalization;

namespace KittyToken.Maui.Converters;

internal sealed partial class JwtVisualizationModeToStringConverter :
    BaseConverterOneWay<JwtVisualizationMode, string>
{
    public override string DefaultConvertReturnValue { get; set; } = string.Empty;

    public override string ConvertFrom(JwtVisualizationMode value, CultureInfo? culture)
    {
        return value switch
        {
            JwtVisualizationMode.None
                => "NONE",

            JwtVisualizationMode.JwtDecoded
                => "JWT DECODED",

            _
                => throw new ArgumentOutOfRangeException(
                    nameof(value),
                    value,
                    "Invalid JwtVisualizationMode value.")
        };
    }
}
