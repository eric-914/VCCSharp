using System;
using System.Globalization;
using System.Windows.Data;
using VCCSharp.Enums;

namespace VCCSharp.Converters
{
    [ValueConversion(typeof(JoystickDevices), typeof(bool))]
    public class JoystickDeviceToSelectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (JoystickDevices)value == (JoystickDevices)parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? parameter : null;
        }
    }
}
