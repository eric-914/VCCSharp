using System;
using System.Globalization;
using System.Windows.Data;
using VCCSharp.Enums;

namespace VCCSharp.Converters
{
    [ValueConversion(typeof(JoystickDevices), typeof(bool))]
    public class JoystickDeviceToEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (JoystickDevices)value == (JoystickDevices)parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
