namespace VCCSharp.OpCodes.Tests;

internal partial class OldCpu
{
    private const ushort VTRAP = 0xFFF0;
    private const ushort VSWI3 = 0xFFF2;
    private const ushort VSWI2 = 0xFFF4;
    private const ushort VSWI = 0xFFFA;

    private byte _temp8;
    private ushort _temp16;
    private uint _temp32;

    private byte _postByte;
    private ushort _postWord;

    private int _cycleCounter;

    private bool CC_E { get => (CC & 0x80) != 0; set => CC = (byte)((CC & 0b01111111) | (value ? 1 : 0) << 7); }
    private bool CC_F { get => (CC & 0x40) != 0; set => CC = (byte)((CC & 0b10111111) | (value ? 1 : 0) << 6); }
    private bool CC_H { get => (CC & 0x20) != 0; set => CC = (byte)((CC & 0b11011111) | (value ? 1 : 0) << 5); }
    private bool CC_I { get => (CC & 0x10) != 0; set => CC = (byte)((CC & 0b11101111) | (value ? 1 : 0) << 4); }
    private bool CC_N { get => (CC & 0x08) != 0; set => CC = (byte)((CC & 0b11110111) | (value ? 1 : 0) << 3); }
    private bool CC_Z { get => (CC & 0x04) != 0; set => CC = (byte)((CC & 0b11111011) | (value ? 1 : 0) << 2); }
    private bool CC_V { get => (CC & 0x02) != 0; set => CC = (byte)((CC & 0b11111101) | (value ? 1 : 0) << 1); }
    private bool CC_C { get => (CC & 0x01) != 0; set => CC = (byte)((CC & 0b11111110) | (value ? 1 : 0) << 0); }

    private byte PC_H { get => (byte)(PC_REG >> 8); set => PC_REG = (ushort)((PC_REG & 0x00FF) | (value << 8)); }
    private byte PC_L { get => (byte)(PC_REG & 0xFF); set => PC_REG = (ushort)((PC_REG & 0xFF00) | value); }

    private byte A_REG { get => (byte)(D_REG >> 8); set => D_REG = (ushort)((D_REG & 0x00FF) | (value << 8)); }
    private byte B_REG { get => (byte)(D_REG & 0xFF); set => D_REG = (ushort)((D_REG & 0xFF00) | value); }

    private byte X_H { get => (byte)(X_REG >> 8); set => X_REG = (ushort)((X_REG & 0x00FF) | (value << 8)); }
    private byte X_L { get => (byte)(X_REG & 0xFF); set => X_REG = (ushort)((X_REG & 0xFF00) | value); }

    private byte Y_H { get => (byte)(Y_REG >> 8); set => Y_REG = (ushort)((Y_REG & 0x00FF) | (value << 8)); }
    private byte Y_L { get => (byte)(Y_REG & 0xFF); set => Y_REG = (ushort)((Y_REG & 0xFF00) | value); }

    private byte S_H { get => (byte)(S_REG >> 8); set => S_REG = (ushort)((S_REG & 0x00FF) | (value << 8)); }
    private byte S_L { get => (byte)(S_REG & 0xFF); set => S_REG = (ushort)((S_REG & 0xFF00) | value); }

    private byte U_H { get => (byte)(U_REG >> 8); set => U_REG = (ushort)((U_REG & 0x00FF) | (value << 8)); }
    private byte U_L { get => (byte)(U_REG & 0xFF); set => U_REG = (ushort)((U_REG & 0xFF00) | value); }

    public byte DPA { get => (byte)(DP_REG >> 8); set => DP_REG = (ushort)((DP_REG & 0x00FF) | (value << 8)); }

    private byte GetCC() => CC;
    private void SetCC(byte value) => CC = value;

    private static bool NTEST8(byte value) => value > 0x7F;
    private static bool NTEST16(ushort value) => value > 0x7FFF;

    private static bool ZTEST(byte value) => value == 0;
    private static bool ZTEST(ushort value) => value == 0;

    private static bool OVERFLOW8(bool c, byte a, ushort b, byte r) => ((c ? 1 : 0) ^ (((a ^ b ^ r) >> 7) & 1)) != 0;
    private static bool OVERFLOW16(bool c, uint a, ushort b, ushort r) => ((c ? (byte)1 : (byte)0) ^ (((a ^ b ^ r) >> 15) & 1)) != 0;

    private ushort DPADDRESS(ushort r) => (ushort)(DP_REG | MemRead8(r)); //DIRECT PAGE REGISTER

    private ushort INDADDRESS(ushort address)
    {
        throw new NotImplementedException();
    }

    private byte PUR(int i) => R8[i];
    private void PUR(int i, byte value) => R8[i] = value;

    private ushort PXF(int i) => R16[i];
    private void PXF(int i, ushort value) => R16[i] = value;


    public byte MemRead8(ushort address)
    {
        return _mem.Read(address);
    }

    public void MemWrite8(byte data, ushort address)
    {
        _mem.Write(address, data);
    }

    public ushort MemRead16(ushort address)
    {
        return (ushort)(MemRead8(address) << 8 | MemRead8((ushort)(address + 1)));
    }

    public void MemWrite16(ushort data, ushort address)
    {
        MemWrite8((byte)(data >> 8), address);
        MemWrite8((byte)(data & 0xFF), (ushort)(address + 1));
    }
}
