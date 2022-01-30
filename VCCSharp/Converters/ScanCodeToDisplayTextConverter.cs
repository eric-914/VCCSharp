﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using VCCSharp.Models.Keyboard;

namespace VCCSharp.Converters
{
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
                return Keys[i];
            }

            return Key.None;
        }
    }
}
