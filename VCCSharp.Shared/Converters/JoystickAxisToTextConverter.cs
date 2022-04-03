using DX8;
using System.Globalization;
using System.Windows.Data;

namespace VCCSharp.Shared.Converters
{
    public class JoystickAxisToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IDxJoystickState j)
            {
                return $"{j.Horizontal},{j.Vertical}";
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
