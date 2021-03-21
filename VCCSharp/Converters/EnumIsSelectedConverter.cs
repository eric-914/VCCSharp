using System;
using System.Globalization;
using System.Windows.Data;

namespace VCCSharp.Converters
{
    /// <summary>
    /// Similar to EnumToSelectionConverter, but with ConvertBack disabled.
    /// Use for IsEnabled, IsVisible, etc...
    /// </summary>
    public class EnumIsSelectedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value == (int)parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
