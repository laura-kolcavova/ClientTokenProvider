using CommunityToolkit.Maui.Converters;
using KittyToken.Maui.Models;
using KittyToken.Maui.Resources.Strings;
using System.Globalization;

namespace KittyToken.Maui.Converters;

internal sealed partial class JwtVisualizationModeConverter :
     BaseConverterOneWay<JwtVisualizationMode, string>
{
    public override string DefaultConvertReturnValue { get; set; } = string.Empty;

    public override string ConvertFrom(JwtVisualizationMode value, CultureInfo? culture)
    {
        return value switch
        {
            JwtVisualizationMode.Encoded => JwtVisualizerResources.VisualizationModeEncoded,
            JwtVisualizationMode.Decoded => JwtVisualizerResources.VisualizationModeDecoded,
            _ => value.ToString(),
        };
    }
}
