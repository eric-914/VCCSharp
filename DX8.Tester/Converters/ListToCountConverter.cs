using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace DX8.Tester.Converters
{
    internal class ListToCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IList l)
            {
                return $"{l.Count} count";
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
