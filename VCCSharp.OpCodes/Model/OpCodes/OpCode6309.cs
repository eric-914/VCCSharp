using VCCSharp.OpCodes.Model.Memory;

namespace VCCSharp.OpCodes.Model.OpCodes;

/// <summary>
/// For use on HD6309 specific opcodes
/// <code>🚫 6309 ONLY 🚫</code>
/// </summary>
internal abstract class OpCode6309 : OpCodeBase<HD6309.IState>
{
    private HD6309.IState _cpu;

    protected Memory8Bit M8 { get; }
    protected Memory16Bit M16 { get; }
    protected Memory32Bit M32 { get; }
    protected MemoryDP DIRECT { get; }
    protected MemoryIndexed INDEXED { get; }

    protected HD6309.Exceptions Exceptions { get; }

    protected OpCode6309(HD6309.IState cpu) : base(cpu)
    {
        _cpu = cpu;

        var memory = new HD6309.Memory(cpu);
        M8 = memory.Byte;
        M16 = memory.Word;
        M32 = memory.DWord;
        DIRECT = memory.DP;
        INDEXED = memory.Indexed;

        Exceptions = new HD6309.Exceptions(cpu);
    }

    protected byte A { get => _cpu.A_REG; set => _cpu.A_REG = value; }
    protected byte B { get => _cpu.B_REG; set => _cpu.B_REG = value; }
    protected byte E { get => _cpu.E_REG; set => _cpu.E_REG = value; }
    protected byte F { get => _cpu.F_REG; set => _cpu.F_REG = value; }

    //TODO: See details in IRegisterDP
    protected byte DP { get => _cpu.DP; set => _cpu.DP = value; }

    protected ushort PC { get => _cpu.PC_REG; set => _cpu.PC_REG = value; }
    protected ushort D { get => _cpu.D_REG; set => _cpu.D_REG = value; }
    protected ushort X { get => _cpu.X_REG; set => _cpu.X_REG = value; }
    protected ushort Y { get => _cpu.Y_REG; set => _cpu.Y_REG = value; }
    protected ushort S { get => _cpu.S_REG; set => _cpu.S_REG = value; }
    protected ushort U { get => _cpu.U_REG; set => _cpu.U_REG = value; }

    protected byte PC_L { get => _cpu.PC_L; set => _cpu.PC_L = value; }
    protected byte PC_H { get => _cpu.PC_H; set => _cpu.PC_H = value; }
    protected byte X_L { get => _cpu.X_L; set => _cpu.X_L = value; }
    protected byte X_H { get => _cpu.X_H; set => _cpu.X_H = value; }
    protected byte Y_L { get => _cpu.Y_L; set => _cpu.Y_L = value; }
    protected byte Y_H { get => _cpu.Y_H; set => _cpu.Y_H = value; }
    protected byte U_L { get => _cpu.U_L; set => _cpu.U_L = value; }
    protected byte U_H { get => _cpu.U_H; set => _cpu.U_H = value; }

    protected byte CC { get => _cpu.CC; set => _cpu.CC = value; }

    protected bool CC_C { get => _cpu.CC_C; set => _cpu.CC_C = value; }
    protected bool CC_E { get => _cpu.CC_E; set => _cpu.CC_E = value; }
    protected bool CC_F { get => _cpu.CC_F; set => _cpu.CC_F = value; }
    protected bool CC_H { get => _cpu.CC_H; set => _cpu.CC_H = value; }
    protected bool CC_I { get => _cpu.CC_I; set => _cpu.CC_I = value; }
    protected bool CC_N { get => _cpu.CC_N; set => _cpu.CC_N = value; }
    protected bool CC_V { get => _cpu.CC_V; set => _cpu.CC_V = value; }
    protected bool CC_Z { get => _cpu.CC_Z; set => _cpu.CC_Z = value; }

    protected byte MD { get => _cpu.MD; set => _cpu.MD = value; }

    protected bool MD_NATIVE6309 { get => _cpu.MD_NATIVE6309; }
    protected bool MD_FIRQMODE { get => _cpu.MD_FIRQMODE; }
    protected bool MD_ILLEGAL { set => _cpu.MD_ILLEGAL = value; }
    protected bool MD_ZERODIV { set => _cpu.MD_ZERODIV = value; }
}
