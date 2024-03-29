﻿using System.Globalization;
using System.Windows.Data;

namespace VCCSharp.Shared.Converters
{
    public class AxisScaleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int v)
            {
                return (v << 1) - 3;
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
