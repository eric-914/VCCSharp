using VCCSharp.OpCodes.Model.Memory;
using VCCSharp.OpCodes.Model.Support;
using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.Model.OpCodes;

/// <summary>
/// For use on HD6309 specific opcodes
/// <code>🚫 6309 ONLY 🚫</code>
/// </summary>
internal abstract class OpCode6309 : OpCodeBase<HD6309.IState>
{
    private HD6309.IState _cpu;

    /// <summary>
    /// 8-bit memory access
    /// </summary>
    protected Memory8Bit M8 { get; }

    /// <summary>
    /// 16-bit memory access
    /// </summary>
    protected Memory16Bit M16 { get; }

    /// <summary>
    /// 32-bit memory access
    /// </summary>
    protected Memory32Bit M32 { get; }

    protected MemoryDP DIRECT { get; }
    protected MemoryIndexed INDEXED { get; }

    public IRegisters8Bit R8 { get;}
    public IRegisters16Bit R16 { get; }

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

        R8 = new HD6309.Registers8Bit<HD6309.IState>(cpu);
        R16 = new MC6809.Registers16Bit<HD6309.IState>(cpu);

        Exceptions = new HD6309.Exceptions(cpu);
    }

    /// <summary>
    /// 8-bit register
    /// </summary>
    protected byte A { get => _cpu.A_REG; set => _cpu.A_REG = value; }

    /// <summary>
    /// 8-bit register
    /// </summary>
    protected byte B { get => _cpu.B_REG; set => _cpu.B_REG = value; }

    /// <summary>
    /// 8-bit register
    /// </summary>
    protected byte E { get => _cpu.E_REG; set => _cpu.E_REG = value; }

    /// <summary>
    /// 8-bit register
    /// </summary>
    protected byte F { get => _cpu.F_REG; set => _cpu.F_REG = value; }

    //TODO: See details in IRegisterDP
    public byte DP { get => _cpu.DP; set => _cpu.DP = value; }

    /// <summary>
    /// Program Counter
    /// </summary>
    protected ushort PC { get => _cpu.PC_REG; set => _cpu.PC_REG = value; }

    /// <summary>
    /// 16-bit register <c>A.B</c>
    /// </summary>
    public ushort D { get => _cpu.D_REG; set => _cpu.D_REG = value; }

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
    protected byte U_L { get => _cpu.U_L; set => _cpu.U_L = value; }
    protected byte U_H { get => _cpu.U_H; set => _cpu.U_H = value; }

    /// <summary>
    /// Condition Codes Register
    /// </summary>
    public byte CC { get => _cpu.CC; set => _cpu.CC = value; }

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

    /// <summary>
    /// 16-bit register <c>E.F</c>
    /// </summary>
    public ushort W { get => _cpu.W_REG; set => _cpu.W_REG = value; }

    /// <summary>
    /// 32-bit register <c>D.W</c> or <c>A.B.E.F</c>
    /// </summary>
    protected uint Q { get => _cpu.Q_REG; set => _cpu.Q_REG = value; }

    /// <summary>
    /// Mode Register
    /// </summary>
    protected byte MD { get => _cpu.MD; set => _cpu.MD = value; }

    /// <summary>
    /// <c>(NM)</c> Native Mode (reduced cycles, W stacked on interrupts)
    /// </summary>
    protected bool MD_NATIVE6309 { get => _cpu.MD_NATIVE6309; }

    /// <summary>
    /// <c>(FM)</c> FIRQ uses IRQ stacking method (Entire state)
    /// </summary>
    protected bool MD_FIRQMODE { get => _cpu.MD_FIRQMODE; }

    /// <summary>
    /// <c>(IL)</c> Illegal Instruction Exception
    /// </summary>
    protected bool MD_ILLEGAL { set => _cpu.MD_ILLEGAL = value; }

    /// <summary>
    /// <c>(/0)</c> Divide-by-zero Exception
    /// </summary>
    protected bool MD_ZERODIV { set => _cpu.MD_ZERODIV = value; }

    /// <summary>
    /// Handles the intracies of calculating the sum two values: <c>a+b</c>
    /// </summary>
    /// <param name="a">first 8-bit</param>
    /// <param name="b">second 8-bit</param>
    /// <returns>object with summation results</returns>
    protected Sum Add(byte a, byte b) => new(a, b);

    /// <summary>
    /// Handles the intracies of calculating the sum two values: <c>a+b</c>
    /// </summary>
    /// <param name="a">first 16-bit</param>
    /// <param name="b">second 16-bit</param>
    /// <returns>object with summation results</returns>
    protected Sum Add(ushort a, ushort b) => new(a, b);

    /// <summary>
    /// Handles the intracies of calculating the difference two values: <c>a-b</c>
    /// </summary>
    /// <param name="a">first 16-bit</param>
    /// <param name="b">second 16-bit</param>
    /// <returns>object with summation results</returns>
    protected Sum Subtract(byte a, byte b) => new(a, b.TwosComplement()); //--Take advantage of: a-b ⇔ a+(-b)

    /// <summary>
    /// Handles the intracies of calculating the difference two values: <c>a-b</c>
    /// </summary>
    /// <param name="a">first 16-bit</param>
    /// <param name="b">second 16-bit</param>
    /// <returns>object with summation results</returns>
    protected Sum Subtract(ushort a, ushort b) => new(a, b.TwosComplement()); //--Take advantage of: a-b ⇔ a+(-b)

    protected Support.Boolean Boolean(byte result) => new(result);
    protected Support.Boolean Boolean(ushort result) => new(result);
}
