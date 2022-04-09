using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;
using VCCSharp.Models.Keyboard;

namespace VCCSharp.Converters;

public class ScanCodeToDisplayTextConverter : IValueConverter
{
    private static readonly List<Key> Keys = KeyScanMapper.KeyIndexes.ToList();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Key k)
        {
            return Keys.IndexOf(k);
        }

        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int i)
        {
            return i != -1 ? Keys[i] : Key.None;
        }

        return Key.None;
    }
}
