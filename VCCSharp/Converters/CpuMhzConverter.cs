using System;
using System.Globalization;
using System.Windows.Data;

namespace VCCSharp.Converters
{
    public class CpuMhzConverter : IMultiValueConverter 
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //--Values may not be string/double during the XAML editor
            if (values[0] is string && values[1] is double)
            {
                string cpu = (string)values[0];
                double mhz = (double) values[1];

                return $"{cpu} @ {mhz:0.000}Mhz";
            }

            return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
