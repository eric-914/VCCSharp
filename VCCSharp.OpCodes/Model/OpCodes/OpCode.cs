using VCCSharp.OpCodes.Model.Memory;
using VCCSharp.OpCodes.Model.Support;
using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.Model.OpCodes;

/// <summary>
/// Most OpCodes are 6809 compatible, so this is the default OpCode base class.
/// </summary>
internal abstract class OpCode 
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

    /// <summary>
    /// Index accessor for 8-bit registers
    /// </summary>
    protected IRegisters8Bit R8 { get; }

    /// <summary>
    /// Index accessor for 16-bit registers
    /// </summary>
    protected IRegisters16Bit R16 { get; }

    protected DynamicCycles DynamicCycles { get; }

    protected OpCode(MC6809.IState cpu) 
    {
        _cpu = cpu;

        var memory = new MC6809.Memory(cpu);
        M8 = memory.Byte;
        M16 = memory.Word;
        DIRECT = memory.DP;
        INDEXED = memory.Indexed;

        R8 = new MC6809.Registers8Bit<MC6809.IState>(cpu);
        R16 = new MC6809.Registers16Bit<MC6809.IState>(cpu);

        DynamicCycles = new DynamicCycles(cpu);
    }

    protected bool IsInInterrupt { get => _cpu.IsInInterrupt; set => _cpu.IsInInterrupt = value; }

    protected bool IsSyncWaiting { get => _cpu.IsSyncWaiting; set => _cpu.IsSyncWaiting = value; }

    protected int SyncCycle { get => _cpu.SyncCycle; set => _cpu.SyncCycle = value; }

    /// <summary>
    /// 8-bit register
    /// </summary>
    protected byte A { get => _cpu.A; set => _cpu.A = value; }

    /// <summary>
    /// 8-bit register
    /// </summary>
    protected byte B { get => _cpu.B; set => _cpu.B = value; }

    //TODO: See details in IRegisterDP
    protected byte DP { get => _cpu.DP; set => _cpu.DP = value; }

    /// <summary>
    /// Program Counter
    /// </summary>
    protected ushort PC { get => _cpu.PC; set => _cpu.PC = value; }

    /// <summary>
    /// 16-bit register <c>A.B</c>
    /// </summary>
    protected ushort D { get => _cpu.D; set => _cpu.D = value; }

    /// <summary>
    /// 16-bit register
    /// </summary>
    protected ushort X { get => _cpu.X; set => _cpu.X = value; }

    /// <summary>
    /// 16-bit register
    /// </summary>
    protected ushort Y { get => _cpu.Y; set => _cpu.Y = value; }

    /// <summary>
    /// 16-bit <c>STACK</c> register
    /// </summary>
    protected ushort S { get => _cpu.S; set => _cpu.S = value; }

    /// <summary>
    /// 16-bit <c>USER-STACK</c> register
    /// </summary>
    protected ushort U { get => _cpu.U; set => _cpu.U = value; }

    /// <summary>
    /// <c>PC</c> low 8 bits
    /// </summary>
    protected byte PC_L { get => PC.L(); set => PC = PC.L(value); }

    /// <summary>
    /// <c>PC</c> high 8 bits
    /// </summary>
    protected byte PC_H { get => PC.H(); set => PC = PC.H(value); }

    /// <summary>
    /// <c>X</c> low 8 bits
    /// </summary>
    protected byte X_L { get => X.L(); set => X = X.L(value); }

    /// <summary>
    /// <c>X</c> high 8 bits
    /// </summary>
    protected byte X_H { get => X.H(); set => X = X.H(value); }

    /// <summary>
    /// <c>Y</c> low 8 bits
    /// </summary>
    protected byte Y_L { get => Y.L(); set => Y = Y.L(value); }

    /// <summary>
    /// <c>Y</c> high 8 bits
    /// </summary>
    protected byte Y_H { get => Y.H(); set => Y = Y.H(value); }

    /// <summary>
    /// <c>S</c> low 8 bits
    /// </summary>
    protected byte S_L { get => S.L(); set => S = S.L(value); }

    /// <summary>
    /// <c>S</c> high 8 bits
    /// </summary>
    protected byte S_H { get => S.H(); set => S = S.H(value); }

    /// <summary>
    /// <c>U</c> low 8 bits
    /// </summary>
    protected byte U_L { get => U.L(); set => U = U.L(value); }

    /// <summary>
    /// <c>U</c> high 8 bits
    /// </summary>
    protected byte U_H { get => U.H(); set => U = U.H(value); }

    /// <summary>
    /// Condition Codes Register
    /// </summary>
    protected byte CC { get => _cpu.CC; set => _cpu.CC = value; }

    /// <summary>
    /// Condition Code Carry Flag
    /// </summary>
    protected bool CC_C { get => _cpu.CC.BitC(); set => _cpu.CC = _cpu.CC.BitC(value); }

    /// <summary>
    /// Condition Code Entire Register State Stacked Flag
    /// </summary>
    protected bool CC_E { get => _cpu.CC.BitE(); set => _cpu.CC = _cpu.CC.BitE(value); }

    /// <summary>
    /// Condition Code FIRQ Flag
    /// </summary>
    protected bool CC_F { get => _cpu.CC.BitF(); set => _cpu.CC = _cpu.CC.BitF(value); }

    /// <summary>
    /// Condition Code Half-Carry Flag
    /// </summary>
    protected bool CC_H { get => _cpu.CC.BitH(); set => _cpu.CC = _cpu.CC.BitH(value); }

    /// <summary>
    /// Condition Code IRQ Flag
    /// </summary>
    protected bool CC_I { get => _cpu.CC.BitI(); set => _cpu.CC = _cpu.CC.BitI(value); }

    /// <summary>
    /// Condition Code Negative Flag
    /// </summary>
    protected bool CC_N { get => _cpu.CC.BitN(); set => _cpu.CC = _cpu.CC.BitN(value); }

    /// <summary>
    /// Condition Code Overflow Flag
    /// </summary>
    protected bool CC_V { get => _cpu.CC.BitV(); set => _cpu.CC = _cpu.CC.BitV(value); }

    /// <summary>
    /// Condition Code Zero Flag
    /// </summary>
    protected bool CC_Z { get => _cpu.CC.BitZ(); set => _cpu.CC = _cpu.CC.BitZ(value); }

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
}
