namespace VCCSharp.OpCodes
{
    internal static class Extensions
    {
        public static byte H(this ushort w) => (byte)(w >> 8);
        public static byte L(this ushort w) => (byte)(w & 0xFF);
        public static ushort H(this ushort w, byte b) => (ushort)((w & 0xFF00) | (b << 8));
        public static ushort L(this ushort w, byte b) => (ushort)((w & 0x00FF) | b);

        public static bool Bit7(this byte b) => (b & 0x80) != 0;
        public static bool Bit6(this byte b) => (b & 0x40) != 0;
        public static bool Bit5(this byte b) => (b & 0x20) != 0;
        public static bool Bit4(this byte b) => (b & 0x10) != 0;
        public static bool Bit3(this byte b) => (b & 0x08) != 0;
        public static bool Bit2(this byte b) => (b & 0x04) != 0;
        public static bool Bit1(this byte b) => (b & 0x02) != 0;
        public static bool Bit0(this byte b) => (b & 0x01) != 0;

        public static bool Bit15(this ushort w) => (w & 0x8000) != 0;
        public static bool Bit14(this ushort w) => (w & 0x4000) != 0;
        public static bool Bit8(this ushort w) => (w & 0x0100) != 0;
        public static bool Bit7(this ushort w) => (w & 0x0080) != 0;
        public static bool Bit0(this ushort w) => (w & 0x0001) != 0;

        public static bool Bit31(this uint d) => (d & 0x80000000) != 0;

        public static byte TwosComplement(this byte b) => (byte)(~b + 1);
        public static ushort TwosComplement(this ushort w) => (ushort)(~w + 1);

        public static byte ToBit(this bool b) => b ? (byte)1 : (byte)0;
        public static byte ToSetMask(this byte b) => (byte)(1 << b);
        public static byte ToClearMask(this byte b) => (byte)~(1 << b);

        public static byte Plus(this byte value, bool bit) => (byte)(value + bit.ToBit());
        public static ushort Plus(this ushort value, bool bit) => (ushort )(value + bit.ToBit());

    }
}
