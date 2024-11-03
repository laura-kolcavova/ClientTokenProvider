using CommunityToolkit.Maui.Converters;
using System.Globalization;

namespace ClientTokenProvider.Shared.Converters;

// string cannot be used as a target type because of possible bug in MAUI.
public sealed class TextChangedEventArgsToNewValueConverter : BaseConverterOneWay<TextChangedEventArgs, object>
{
    public override object DefaultConvertReturnValue { get; set; } = string.Empty;

    public override object ConvertFrom(TextChangedEventArgs value, CultureInfo? culture)
    {
        return value.NewTextValue;
    }
}
