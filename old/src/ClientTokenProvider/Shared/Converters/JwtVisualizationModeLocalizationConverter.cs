using ClientTokenProvider.Resources.Strings.Shared;
using ClientTokenProvider.Shared.Models;
using CommunityToolkit.Maui.Converters;
using System.Globalization;

namespace ClientTokenProvider.Shared.Converters;

internal sealed class JwtVisualizationModeLocalizationConverter :
       BaseConverterOneWay<JwtVisualizationMode, string>
{
    public override string DefaultConvertReturnValue { get; set; } = string.Empty;

    public override string ConvertFrom(JwtVisualizationMode value, CultureInfo? culture)
    {
        return value switch
        {
            JwtVisualizationMode.None
                => SharedStrings.JwtVisualizationMode_None,

            JwtVisualizationMode.JwtDecoded
                => SharedStrings.JwtVisualizationMode_JwtDecoded,

            _
                => throw new ArgumentOutOfRangeException(
                    nameof(value),
                    value,
                    "Invalid JwtVisualizationMode value.")
        };
    }
}
