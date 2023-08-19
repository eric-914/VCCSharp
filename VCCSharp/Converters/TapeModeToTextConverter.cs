using System.Globalization;
using System.Windows.Data;
using VCCSharp.Configuration.Options;

namespace VCCSharp.Converters;

public class TapeModeToTextConverter : IValueConverter
{
    public static readonly Dictionary<TapeModes, string> ModeTexts = new()
    {
        {TapeModes.Eject, "Eject"},
        {TapeModes.Play, "Play"},
        {TapeModes.Record, "Record"},
        {TapeModes.Stop, "Stop"},
    };

    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null) return string.Empty;

        var mode = (TapeModes)value;

        return ModeTexts[mode];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
