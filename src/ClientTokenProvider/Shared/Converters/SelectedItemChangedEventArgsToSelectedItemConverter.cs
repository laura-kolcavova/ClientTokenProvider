using CommunityToolkit.Maui.Converters;
using System.Globalization;

namespace ClientTokenProvider.Shared.Converters;


internal sealed class SelectedItemChangedEventArgsToSelectedItemConverter : BaseConverterOneWay<SelectedItemChangedEventArgs, object>
{
    public override object DefaultConvertReturnValue { get; set; } = null!;

    public override object ConvertFrom(SelectedItemChangedEventArgs value, CultureInfo? culture)
    {
        return value.SelectedItem;
    }
}
