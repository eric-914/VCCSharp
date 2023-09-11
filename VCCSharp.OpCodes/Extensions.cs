using VCCSharp.OpCodes.Definitions;
using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes;

internal static class Extensions
{
    public static byte H(this ushort w) => (byte)(w >> 8);
    public static byte L(this ushort w) => (byte)(w & 0xFF);
    public static ushort H(this ushort w, byte b) => (ushort)((w & 0x00FF) | (b << 8));
    public static ushort L(this ushort w, byte b) => (ushort)((w & 0xFF00) | b);

    //--Invert the bits
    public static byte I(this byte b) => (byte)(0xFF - b);
    public static ushort I(this ushort b) => (ushort)(0xFFFF - b);
    public static int I(this int b) => (int)(0xFFFFFFFF - (uint)b);

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

    public static bool Bit31(this int b) => (b & 0x80000000) != 0;
    public static bool Bit15(this int b) => (b & 0x8000) != 0;
    public static bool Bit7(this int b) => (b & 0x0080) != 0;

    #region CC Register

    public static bool BitE(this byte b) => (b & (byte)CC_mask.E) != 0;
    public static bool BitF(this byte b) => (b & (byte)CC_mask.F) != 0;
    public static bool BitH(this byte b) => (b & (byte)CC_mask.H) != 0;
    public static bool BitI(this byte b) => (b & (byte)CC_mask.I) != 0;
    public static bool BitN(this byte b) => (b & (byte)CC_mask.N) != 0;
    public static bool BitZ(this byte b) => (b & (byte)CC_mask.Z) != 0;
    public static bool BitV(this byte b) => (b & (byte)CC_mask.V) != 0;
    public static bool BitC(this byte b) => (b & (byte)CC_mask.C) != 0;

    public static byte BitE(this byte b, bool v) => (byte)((b & ~(byte)CC_mask.E) | (v.ToBit() << (byte)CC_bits.E));
    public static byte BitF(this byte b, bool v) => (byte)((b & ~(byte)CC_mask.F) | (v.ToBit() << (byte)CC_bits.F));
    public static byte BitH(this byte b, bool v) => (byte)((b & ~(byte)CC_mask.H) | (v.ToBit() << (byte)CC_bits.H));
    public static byte BitI(this byte b, bool v) => (byte)((b & ~(byte)CC_mask.I) | (v.ToBit() << (byte)CC_bits.I));
    public static byte BitN(this byte b, bool v) => (byte)((b & ~(byte)CC_mask.N) | (v.ToBit() << (byte)CC_bits.N));
    public static byte BitZ(this byte b, bool v) => (byte)((b & ~(byte)CC_mask.Z) | (v.ToBit() << (byte)CC_bits.Z));
    public static byte BitV(this byte b, bool v) => (byte)((b & ~(byte)CC_mask.V) | (v.ToBit() << (byte)CC_bits.V));
    public static byte BitC(this byte b, bool v) => (byte)((b & ~(byte)CC_mask.C) | (v.ToBit() << (byte)CC_bits.C));

    #endregion

    #region MD Register

    public static bool Bit_NATIVE6309(this byte b) => (b & (byte)MD_mask.NATIVE6309) != 0;
    public static bool Bit_FIRQMODE(this byte b) => (b & (byte)MD_mask.FIRQMODE) != 0;

    public static byte Bit_ILLEGAL(this byte b, bool v) => (byte)((b & ~(byte)MD_mask.ILLEGAL) | (v.ToBit() << (byte)MD_bits.ILLEGAL));
    public static byte Bit_ZERODIV(this byte b, bool v) => (byte)((b & ~(byte)MD_mask.ZERODIV) | (v.ToBit() << (byte)MD_bits.ZERODIV));

    #endregion

    public static byte TwosComplement(this byte b) => (byte)(~b + 1);
    public static ushort TwosComplement(this ushort w) => (ushort)(~w + 1);

    public static byte ToBit(this bool b) => b ? (byte)1 : (byte)0;
    public static byte ToSetMask(this byte b) => (byte)(1 << b);
    public static byte ToClearMask(this byte b) => (byte)~(1 << b);

    #region Register D ⇔ Register A (High) | Register B (Low)

    public static byte A(this IRegisterD d) => d.D.H();
    public static void A(this IRegisterD d, byte value) => d.D = d.D.H(value);
    public static byte B(this IRegisterD d) => d.D.L();
    public static void B(this IRegisterD d, byte value) => d.D = d.D.L(value);

    #endregion

    #region Register W ⇔ Register E (High) | Register F (Low)

    public static byte E(this IRegisterW w) => w.W.H();
    public static void E(this IRegisterW w, byte value) => w.W = w.W.H(value);
    public static byte F(this IRegisterW w) => w.W.L();
    public static void F(this IRegisterW w, byte value) => w.W = w.W.L(value);

    #endregion
}
