using System.Linq;
using System.Text;
using DX8.Models;
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

        public static byte[] ToByteArray(string text)
        {
            return Encoding.ASCII.GetBytes(text);
        }
    }
}
