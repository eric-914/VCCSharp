using VCCSharp.OpCodes.MC6809;
using VCCSharp.OpCodes.Model.Memory;
using VCCSharp.OpCodes.Model.Support;
using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.Model.OpCodes;

/// <summary>
/// Most OpCodes are 6809 compatible, so this is the default OpCode base class.
/// </summary>
internal abstract class OpCode 
{
    private readonly ISystemState _ss;

    protected OpCode(ISystemState ss) => _ss = ss;

    protected OpCode(IState cpu) : this(new SystemState(cpu)) { }

    private IState cpu => _ss.cpu;

    /// <summary>
    /// 8-bit memory access
    /// </summary>
    protected Memory8Bit M8 => _ss.M8;

    /// <summary>
    /// 16-bit memory access
    /// </summary>
    protected Memory16Bit M16 => _ss.M16;

    protected MemoryDP DIRECT => _ss.DIRECT;

    /// <summary>
    /// 8-bit "Effective Address" memory access
    /// </summary>
    protected MemoryIndexed INDEXED => _ss.INDEXED;

    /// <summary>
    /// Index accessor for 8-bit registers
    /// </summary>
    public IRegisters8Bit R8 => _ss.R8;

    /// <summary>
    /// Index accessor for 16-bit registers
    /// </summary>
    public IRegisters16Bit R16 => _ss.R16;

    protected DynamicCycles DynamicCycles => _ss.DynamicCycles;

    protected bool IsInInterrupt { get => cpu.IsInInterrupt; set => cpu.IsInInterrupt = value; }

    protected bool IsSyncWaiting { get => cpu.IsSyncWaiting; set => cpu.IsSyncWaiting = value; }

    protected int SyncCycle { get => cpu.SyncCycle; set => cpu.SyncCycle = value; }

    /// <summary>
    /// 8-bit register
    /// </summary>
    protected byte A { get => cpu.A; set => cpu.A = value; }

    /// <summary>
    /// 8-bit register
    /// </summary>
    protected byte B { get => cpu.B; set => cpu.B = value; }

    //TODO: See details in IRegisterDP
    protected byte DP { get => cpu.DP; set => cpu.DP = value; }

    /// <summary>
    /// Program Counter
    /// </summary>
    protected ushort PC { get => cpu.PC; set => cpu.PC = value; }

    /// <summary>
    /// 16-bit register <c>A.B</c>
    /// </summary>
    protected ushort D { get => cpu.D; set => cpu.D = value; }

    /// <summary>
    /// 16-bit register
    /// </summary>
    protected ushort X { get => cpu.X; set => cpu.X = value; }

    /// <summary>
    /// 16-bit register
    /// </summary>
    protected ushort Y { get => cpu.Y; set => cpu.Y = value; }

    /// <summary>
    /// 16-bit <c>STACK</c> register
    /// </summary>
    protected ushort S { get => cpu.S; set => cpu.S = value; }

    /// <summary>
    /// 16-bit <c>USER-STACK</c> register
    /// </summary>
    protected ushort U { get => cpu.U; set => cpu.U = value; }

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
    protected byte CC { get => cpu.CC; set => cpu.CC = value; }

    /// <summary>
    /// Condition Code Carry Flag
    /// </summary>
    protected bool CC_C { get => cpu.CC.BitC(); set => cpu.CC = cpu.CC.BitC(value); }

    /// <summary>
    /// Condition Code Entire Register State Stacked Flag
    /// </summary>
    protected bool CC_E { get => cpu.CC.BitE(); set => cpu.CC = cpu.CC.BitE(value); }

    /// <summary>
    /// Condition Code FIRQ Flag
    /// </summary>
    protected bool CC_F { get => cpu.CC.BitF(); set => cpu.CC = cpu.CC.BitF(value); }

    /// <summary>
    /// Condition Code Half-Carry Flag
    /// </summary>
    protected bool CC_H { get => cpu.CC.BitH(); set => cpu.CC = cpu.CC.BitH(value); }

    /// <summary>
    /// Condition Code IRQ Flag
    /// </summary>
    protected bool CC_I { get => cpu.CC.BitI(); set => cpu.CC = cpu.CC.BitI(value); }

    /// <summary>
    /// Condition Code Negative Flag
    /// </summary>
    protected bool CC_N { get => cpu.CC.BitN(); set => cpu.CC = cpu.CC.BitN(value); }

    /// <summary>
    /// Condition Code Overflow Flag
    /// </summary>
    protected bool CC_V { get => cpu.CC.BitV(); set => cpu.CC = cpu.CC.BitV(value); }

    /// <summary>
    /// Condition Code Zero Flag
    /// </summary>
    protected bool CC_Z { get => cpu.CC.BitZ(); set => cpu.CC = cpu.CC.BitZ(value); }

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
