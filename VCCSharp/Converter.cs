using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using VCCSharp.Models;

namespace VCCSharp
{
    public static class Converter
    {
        public static string ToString(byte[] buffer)
        {
            return Encoding.ASCII.GetString(buffer, 0, buffer.Length).Split('\0').First();
        }

        public static unsafe string ToString(byte* source, int max = Define.MAX_LOADSTRING)
        {
            byte[] buffer = new byte[max];

            for (int index = 0; index < max && source[index] != '\0'; index++)
            {
                buffer[index] = source[index];
            }

            return Encoding.ASCII.GetString(buffer, 0, buffer.Length).Split('\0').First();
        }

        public static string ToString(IntPtr source, int max = Define.MAX_LOADSTRING)
        {
            var buffer = new byte[max];

            Marshal.Copy(source, buffer, 0, max);

            return ToString(buffer);
        }

        public static byte[] ToByteArray(string text)
        {
            return Encoding.ASCII.GetBytes(text);
        }
    }
}
