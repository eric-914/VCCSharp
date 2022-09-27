using System.Globalization;
using System.Windows.Data;

namespace VCCSharp.Converters;

public class FrameSkipConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return $"Skip: {value}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
