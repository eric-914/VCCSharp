using System;
using System.Globalization;
using System.Windows.Data;

namespace VCCSharp.Converters
{
    public class ScanCodeToDisplayTextConverter : IValueConverter
    {
        private static readonly int[] TranslateScan2Disp = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 32, 38, 20, 33, 35, 40, 36, 24, 30, 31, 42, 43, 55, 52, 16, 34, 19, 21, 22, 23, 25, 26, 27, 45, 46, 0, 51, 44, 41, 39, 18, 37, 17, 29, 28, 47, 48, 49, 51, 0, 53, 54, 50, 66, 67, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 58, 64, 60, 0, 62, 0, 63, 0, 59, 65, 61, 56, 57 };
        private static readonly int[] TranslateDisp2Scan = { 78, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 30, 48, 46, 32, 18, 33, 34, 35, 23, 36, 37, 38, 50, 49, 24, 25, 16, 19, 31, 20, 22, 47, 17, 45, 21, 44, 26, 27, 43, 39, 40, 51, 52, 53, 58, 54, 29, 56, 57, 28, 82, 83, 71, 79, 73, 81, 75, 77, 72, 80, 59, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int index = (byte)value;
            index = index < 0 ? 0 : index;
            return TranslateScan2Disp[index];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int index = (int) value;
            index = index < 0 ? 0 : index;
            return (char)TranslateDisp2Scan[index];
        }
    }
}
