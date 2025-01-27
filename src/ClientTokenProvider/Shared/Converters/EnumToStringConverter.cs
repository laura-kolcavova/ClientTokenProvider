using CommunityToolkit.Maui.Converters;
using System.Globalization;

namespace ClientTokenProvider.Shared.Converters;

internal sealed class EnumToStringConverter :
    BaseConverterOneWay<Enum, string>
{
    public override string DefaultConvertReturnValue { get; set; } = string.Empty;

    public override string ConvertFrom(Enum value, CultureInfo? culture)
    {
        return Enum.GetName(value.GetType(), value) ?? string.Empty;
    }
}
