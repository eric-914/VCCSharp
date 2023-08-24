namespace VCCSharp.OpCodes
{
    internal static class Extensions
    {
        public static byte H(this ushort source) => (byte)(source >> 8);
        public static byte L(this ushort source) => (byte)(source & 0xFF);

        public static bool IsNegative(this byte source) => source > 0x7F;

        public static bool IsZero(this byte source) => source == 0;

        public static bool Bit7(this byte source) => (source & 0x80) != 0;
        public static bool Bit6(this byte source) => (source & 0x40) != 0;
        public static bool Bit5(this byte source) => (source & 0x20) != 0;
        public static bool Bit4(this byte source) => (source & 0x10) != 0;
        public static bool Bit3(this byte source) => (source & 0x08) != 0;
        public static bool Bit2(this byte source) => (source & 0x04) != 0;
        public static bool Bit1(this byte source) => (source & 0x02) != 0;
        public static bool Bit0(this byte source) => (source & 0x01) != 0;

        public static bool Bit15(this ushort source) => (source & 0x8000) != 0;

        public static byte ToByte(this bool source) => source ? (byte)1 : (byte)0;
    }
}
