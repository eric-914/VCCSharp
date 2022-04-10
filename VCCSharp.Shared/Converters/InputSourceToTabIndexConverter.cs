using System.Globalization;
using System.Windows.Data;
using VCCSharp.Shared.Enums;

namespace VCCSharp.Shared.Converters;

public class InputSourceToTabIndexConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is JoystickDevices v)
        {
            return (int)v;
        }

        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int v)
        {
            return (JoystickDevices)v;
        }

        return JoystickDevices.Keyboard;
    }
}
