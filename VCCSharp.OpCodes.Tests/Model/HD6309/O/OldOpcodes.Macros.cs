using VCCSharp.OpCodes.Definitions;

namespace VCCSharp.OpCodes.Tests.Model.HD6309.O;

internal partial class OldOpcodes
{
    private const ushort VTRAP = 0xFFF0;
    private const ushort VSWI3 = 0xFFF2;
    private const ushort VSWI2 = 0xFFF4;
    private const ushort VSWI = 0xFFFA;

    public Mode Mode { get; set; } = Mode.MC6809;

    private byte _temp8;
    private ushort _temp16;
    private uint _temp32;

    private byte _postByte;
    private ushort _postWord;

    private int _cycleCounter;

    private byte _source;
    private byte _dest;
    private short _signedShort;
    private int _signedInt;

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

    public bool MD_NATIVE6309 { get => MD.Bit_NATIVE6309(); }
    public bool MD_ZERODIV { set => MD = MD.Bit_ZERODIV(value); }
    public bool MD_ILLEGAL { set => MD = MD.Bit_ILLEGAL(value); }

    private byte E_REG { get => (byte)(W_REG >> 8); set => W_REG = (ushort)((W_REG & 0x00FF) | (value << 8)); }
    private byte F_REG { get => (byte)(W_REG & 0xFF); set => W_REG = (ushort)((W_REG & 0xFF00) | value); }

    public ushort D_REG { get => (ushort)(Q_REG >> 16); set => Q_REG = (Q_REG & 0x0000FFFF) | (uint)(value << 16); } // A | B
    public ushort W_REG { get => (ushort)(Q_REG & 0xFFFF); set => Q_REG = (Q_REG & 0xFFFF0000) | value; } // E | F

    private byte Z_REG { get => 0; set { } }

    private byte GetCC() => CC;
    private void SetCC(byte value) => CC = value;

    private static bool NTEST8(byte value) => value > 0x7F;
    private static bool NTEST16(ushort value) => value > 0x7FFF;
    private static bool NTEST32(uint value) => value > 0x7FFFFFFF;

    private static bool ZTEST(byte value) => value == 0;
    private static bool ZTEST(ushort value) => value == 0;
    private static bool ZTEST(uint value) => value == 0;

    private static bool OVERFLOW8(bool c, byte a, ushort b, byte r) => ((c ? 1 : 0) ^ (((a ^ b ^ r) >> 7) & 1)) != 0;
    private static bool OVERFLOW16(bool c, uint a, ushort b, ushort r) => ((c ? (byte)1 : (byte)0) ^ (((a ^ b ^ r) >> 15) & 1)) != 0;

    private ushort DPADDRESS(ushort r) => (ushort)(DP_REG | MemRead8(r)); //DIRECT PAGE REGISTER

    private ushort INDADDRESS(ushort address) => CalculateEA(MemRead8(address));

    private byte PUR(int i)
    {
        return i switch
        {
            0 => A_REG,
            1 => B_REG,
            2 => CC,
            3 => DPA,
            4 => Z_REG,
            5 => Z_REG,
            6 => E_REG,
            7 => F_REG,
            _ => throw new NotImplementedException()
        };
    }

    private void PUR(int i, byte value)
    {
        switch (i)
        {
            case 0: A_REG = value; break;
            case 1: B_REG = value; break;
            case 2: CC = value; break;
            case 3: DPA = value; break;
            case 4: Z_REG = value; break;
            case 5: Z_REG = value; break;
            case 6: E_REG = value; break;
            case 7: F_REG = value; break;
        }
    }

    private ushort PXF(int i)
    {
        return i switch
        {
            0 => D_REG,
            1 => X_REG,
            2 => Y_REG,
            3 => U_REG,
            4 => S_REG,
            5 => PC_REG,
            6 => W_REG,
            7 => V_REG,
            _ => throw new NotImplementedException()
        };
    }

    private void PXF(int i, ushort value)
    {
        switch (i)
        {
            case 0: D_REG = value; break;
            case 1: X_REG = value; break;
            case 2: Y_REG = value; break;
            case 3: U_REG = value; break;
            case 4: S_REG = value; break;
            case 5: PC_REG = value; break;
            case 6: W_REG = value; break;
            case 7: V_REG = value; break;
            default: break;
        }
    }

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

    public uint MemRead32(ushort address)
    {
        return (ushort)(MemRead16(address) << 16 | MemRead16((ushort)(address + 2)));
    }

    public void MemWrite32(uint data, ushort address)
    {
        MemWrite16((byte)(data >> 16), address);
        MemWrite16((byte)(data & 0xFFFF), (ushort)(address + 2));
    }

    public void DivByZero()
    {
        MD_ZERODIV = true;

        ErrorVector();
    }

    public void IllegalInstruction()
    {
        MD_ILLEGAL = true;

        ErrorVector();
    }

    private void ErrorVector()
    {
        CC_E = true; //1;

        MemWrite8(PC_L, --S_REG);
        MemWrite8(PC_H, --S_REG);
        MemWrite8(U_L, --S_REG);
        MemWrite8(U_H, --S_REG);
        MemWrite8(Y_L, --S_REG);
        MemWrite8(Y_H, --S_REG);
        MemWrite8(X_L, --S_REG);
        MemWrite8(X_H, --S_REG);
        MemWrite8(DPA, --S_REG);

        if (MD_NATIVE6309)
        {
            MemWrite8(F_REG, --S_REG);
            MemWrite8(E_REG, --S_REG);

            _cycleCounter += 2;
        }

        MemWrite8(B_REG, --S_REG);
        MemWrite8(A_REG, --S_REG);
        MemWrite8(GetCC(), --S_REG);

        PC_REG = MemRead16(Define.VTRAP);

        _cycleCounter += 12 + _instance._54;	//One for each byte +overhead? Guessing from PSHS
    }
}
