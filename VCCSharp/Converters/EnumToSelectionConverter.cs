using System;
using System.Globalization;
using System.Windows.Data;

namespace VCCSharp.Converters
{
    public class EnumToSelectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int v && parameter is int p)
            {
                return v == p;
            }

            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is true ? parameter : null;
        }
    }
}
