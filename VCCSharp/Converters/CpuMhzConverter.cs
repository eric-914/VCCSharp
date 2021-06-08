using System;
using System.Globalization;
using System.Windows.Data;

namespace VCCSharp.Converters
{
    public class CpuMhzConverter : IMultiValueConverter 
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string cpu = (string)values[0];
            double mhz = (double) values[1];

            return $"{cpu} @ {mhz:0.000}Mhz";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
