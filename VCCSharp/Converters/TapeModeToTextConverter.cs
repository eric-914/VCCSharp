using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using VCCSharp.Enums;

namespace VCCSharp.Converters
{
    public class TapeModeToTextConverter : IValueConverter
    {
        public static readonly Dictionary<TapeModes, string> ModeTexts = new Dictionary<TapeModes, string>
        {
            {TapeModes.EJECT, "EJECT"},
            {TapeModes.PLAY, "PLAY"},
            {TapeModes.REC, "RECORD"},
            {TapeModes.STOP, "STOP"},
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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
}
