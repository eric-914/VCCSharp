using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DX8.Internal.Converters
{
    public static class StringConverter
    {
        public static string ToString(byte[] buffer)
        {
            return Encoding.ASCII.GetString(buffer, 0, buffer.Length).Split('\0').First();
        }

        public static string ToString(IntPtr source, int max = DxDefine.MAX_LOADSTRING)
        {
            var buffer = new byte[max];

            Marshal.Copy(source, buffer, 0, max);

            return Encoding.ASCII.GetString(buffer, 0, buffer.Length).Split('\0').First();
        }
    }
}
