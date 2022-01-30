using System;
using System.Globalization;
using System.Windows.Data;
using VCCSharp.Enums;

namespace VCCSharp.Converters
{
    public class KeyboardLayoutConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is KeyboardLayouts layout)
            {
                return (int)layout;
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int i)
            {
                return (KeyboardLayouts)i;
            }

            return KeyboardLayouts.CoCo;
        }
    }
}
