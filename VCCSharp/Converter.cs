using System.Linq;
using System.Text;

namespace VCCSharp
{
    public static class Converter
    {
        public static string ToString(byte[] buffer)
        {
            return Encoding.ASCII.GetString(buffer, 0, buffer.Length).Split('\0').First();
        }

        public static byte[] ToByteArray(string text)
        {
            return Encoding.ASCII.GetBytes(text);
        }

        public static unsafe void ToByteArray(string text, byte* target)
        {
            byte[] buffer = ToByteArray(text);

            for (int i = 0; i < buffer.Length; i++)
            {
                target[i] = buffer[i];
            }
        }
    }
}
