using System.Globalization;
using System.Windows.Data;

namespace VCCSharp.Converters;

public class DimensionConverter : IMultiValueConverter 
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        //--Values may not be string/double during the XAML editor
        if (values[0] is double && values[1] is double)
        {
            double x = (double)values[0];
            double y = (double) values[1];

            return $"{x:0} x {y:0}";
        }

        return string.Empty;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
