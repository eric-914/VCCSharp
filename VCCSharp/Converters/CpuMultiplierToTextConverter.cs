using System;
using System.Globalization;
using System.Windows.Data;

namespace VCCSharp.Converters
{
    public class CpuMultiplierToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float transform = (float)((int) value * 0.894);

            return $"{transform:F3} Mhz";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
