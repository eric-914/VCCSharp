using System.Globalization;
using System.Windows.Data;

namespace VCCSharp.Converters;

public class EnumToSelectionConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value != null && parameter != null && (int)value == (int)parameter;
    }

    public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is true ? parameter : null;
    }
}
