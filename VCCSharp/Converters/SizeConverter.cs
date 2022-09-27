using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace VCCSharp.Converters;

public class SizeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is Size size ? $"{(int)(size.Width):0} x {(int)(size.Height):0}" : string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
