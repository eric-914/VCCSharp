using System.Linq;
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

        public static byte[] ToByteArray(string text)
        {
            return Encoding.ASCII.GetBytes(text);
        }

        public static unsafe void ToByteArray(string text, byte* target)
        {
            if (string.IsNullOrEmpty(text))
            {
                text = "\0";
            }

            byte[] buffer = ToByteArray(text);

            for (int i = 0; i < buffer.Length; i++)
            {
                target[i] = buffer[i];
            }
        }
    }
}
