using System;
using System.Globalization;
using System.Windows.Data;
using VCCSharp.Enums;

namespace VCCSharp.Converters
{
    [ValueConversion(typeof(JoystickEmulations), typeof(bool))]    
    public class JoystickEmulationToSelectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (JoystickEmulations)value == (JoystickEmulations)parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? parameter : null;
        }
    }
}
