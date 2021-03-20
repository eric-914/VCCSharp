using System;
using System.Globalization;
using System.Windows.Data;
using VCCSharp.Enums;

namespace VCCSharp.Converters
{
    [ValueConversion(typeof(AudioRates), typeof(bool))]
    public class AudioRateToSelectionConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (AudioRates)value == (AudioRates)parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? parameter : null;
        }
    }
}
