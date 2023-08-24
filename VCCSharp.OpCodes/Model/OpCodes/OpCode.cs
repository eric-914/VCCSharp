using VCCSharp.OpCodes.Model.Memory;
using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.Model.OpCodes;

/// <summary>
/// Most OpCodes are 6809 compatible, so this is the default OpCode base class.
/// </summary>
internal abstract class OpCode : OpCodeBase<MC6809.IState>
{
    private MC6809.IState _cpu;

    /// <summary>
    /// 8-bit memory access
    /// </summary>
    protected Memory8Bit M8 { get; }

    /// <summary>
    /// 16-bit memory access
    /// </summary>
    protected Memory16Bit M16 { get; }

    protected MemoryDP DIRECT { get; }

    /// <summary>
    /// 8-bit "Effective Address" memory access
    /// </summary>
    protected MemoryIndexed INDEXED { get; }

    protected IRegisters8Bit R8 { get; }
    protected IRegisters16Bit R16 { get; }

    protected OpCode(MC6809.IState cpu) : base(cpu)
    {
        _cpu = cpu;

        var memory = new MC6809.Memory(cpu);
        M8 = memory.Byte;
        M16 = memory.Word;
        DIRECT = memory.DP;
        INDEXED = memory.Indexed;

        R8 = new MC6809.Registers8Bit<MC6809.IState>(cpu);
        R16 = new MC6809.Registers16Bit<MC6809.IState>(cpu);
    }

    /// <summary>
    /// 8-bit register
    /// </summary>
    protected byte A { get => _cpu.A_REG; set => _cpu.A_REG = value; }

    /// <summary>
    /// 8-bit register
    /// </summary>
    protected byte B { get => _cpu.B_REG; set => _cpu.B_REG = value; }

    //TODO: See details in IRegisterDP
    protected byte DP { get => _cpu.DP; set => _cpu.DP = value; }

    /// <summary>
    /// Program Counter
    /// </summary>
    protected ushort PC { get => _cpu.PC_REG; set => _cpu.PC_REG = value; }

    /// <summary>
    /// 16-bit register <c>A.B</c>
    /// </summary>
    protected ushort D { get => _cpu.D_REG; set => _cpu.D_REG = value; }

    /// <summary>
    /// 16-bit register
    /// </summary>
    protected ushort X { get => _cpu.X_REG; set => _cpu.X_REG = value; }

    /// <summary>
    /// 16-bit register
    /// </summary>
    protected ushort Y { get => _cpu.Y_REG; set => _cpu.Y_REG = value; }

    /// <summary>
    /// 16-bit <c>STACK</c> register
    /// </summary>
    protected ushort S { get => _cpu.S_REG; set => _cpu.S_REG = value; }

    /// <summary>
    /// 16-bit <c>USER-STACK</c> register
    /// </summary>
    protected ushort U { get => _cpu.U_REG; set => _cpu.U_REG = value; }

    protected byte PC_L { get => _cpu.PC_L; set => _cpu.PC_L = value; }
    protected byte PC_H { get => _cpu.PC_H; set => _cpu.PC_H = value; }
    protected byte X_L { get => _cpu.X_L; set => _cpu.X_L = value; }
    protected byte X_H { get => _cpu.X_H; set => _cpu.X_H = value; }
    protected byte Y_L { get => _cpu.Y_L; set => _cpu.Y_L = value; }
    protected byte Y_H { get => _cpu.Y_H; set => _cpu.Y_H = value; }
    protected byte S_L { get => _cpu.S_L; set => _cpu.S_L = value; }
    protected byte S_H { get => _cpu.S_H; set => _cpu.S_H = value; }
    protected byte U_L { get => _cpu.U_L; set => _cpu.U_L = value; }
    protected byte U_H { get => _cpu.U_H; set => _cpu.U_H = value; }

    /// <summary>
    /// Condition Codes Register
    /// </summary>
    protected byte CC { get => _cpu.CC; set => _cpu.CC = value; }

    /// <summary>
    /// Condition Code Carry Flag
    /// </summary>
    protected bool CC_C { get => _cpu.CC_C; set => _cpu.CC_C = value; }

    /// <summary>
    /// Condition Code Entire Register State Stacked Flag
    /// </summary>
    protected bool CC_E { get => _cpu.CC_E; set => _cpu.CC_E = value; }

    /// <summary>
    /// Condition Code FIRQ Flag
    /// </summary>
    protected bool CC_F { get => _cpu.CC_F; set => _cpu.CC_F = value; }

    /// <summary>
    /// Condition Code Half-Carry Flag
    /// </summary>
    protected bool CC_H { get => _cpu.CC_H; set => _cpu.CC_H = value; }

    /// <summary>
    /// Condition Code IRQ Flag
    /// </summary>
    protected bool CC_I { get => _cpu.CC_I; set => _cpu.CC_I = value; }

    /// <summary>
    /// Condition Code Negative Flag
    /// </summary>
    protected bool CC_N { get => _cpu.CC_N; set => _cpu.CC_N = value; }

    /// <summary>
    /// Condition Code Overflow Flag
    /// </summary>
    protected bool CC_V { get => _cpu.CC_V; set => _cpu.CC_V = value; }

    /// <summary>
    /// Condition Code Zero Flag
    /// </summary>
    protected bool CC_Z { get => _cpu.CC_Z; set => _cpu.CC_Z = value; }
}
