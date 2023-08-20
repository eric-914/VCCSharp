using System.Globalization;
using System.Windows.Data;
using VCCSharp.Configuration.Options;

namespace VCCSharp.Converters;

public class AudioRateConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is AudioRates rate)
        {
            return (int)rate;
        }

        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int i)
        {
            return (AudioRates)i;
        }

        return AudioRates.Disabled;
    }
}
