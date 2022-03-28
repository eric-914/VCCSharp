using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace VCCSharp.Converters
{
    internal class BooleanToBorderColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                return b
                    ? Brushes.Blue
                    : Brushes.Transparent;
            }

            return Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
