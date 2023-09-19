using VCCSharp.OpCodes.MC6809;
using VCCSharp.OpCodes.Model.Functions;
using VCCSharp.OpCodes.Model.Memory;
using VCCSharp.OpCodes.Model.Support;
using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.Model.OpCodes;

/// <summary>
/// Most OpCodes are 6809 compatible, so this is the default OpCode base class.
/// </summary>
internal abstract class OpCode
{
    public ISystemState System { get; set; } = default!;

    private IState _state => System.State;

    /// <summary>
    /// 8-bit memory access
    /// </summary>
    protected Memory8Bit M8 => System.M8;

    /// <summary>
    /// 16-bit memory access
    /// </summary>
    protected Memory16Bit M16 => System.M16;

    /// <summary>
    /// 8-bit (DP | offset) memory access
    /// </summary>
    protected MemoryDirect DIRECT => System.DIRECT;

    /// <summary>
    /// 8-bit "Effective Address" memory access
    /// </summary>
    protected MemoryIndexed INDEXED => System.INDEXED;

    /// <summary>
    /// Index accessor for 8-bit registers
    /// </summary>
    public IRegisters8Bit R8 => System.R8;

    /// <summary>
    /// Index accessor for 16-bit registers
    /// </summary>
    public IRegisters16Bit R16 => System.R16;

    protected DynamicCycles DynamicCycles => new DynamicCycles(() => System.Mode);

    /// <summary>
    /// For use on EA/INDEXED memory access as EA has cycle penalties.
    /// </summary>
    public int Cycles { get => System.Cycles; set => System.Cycles = value; }

    protected void ClearInterrupt() => _state.ClearInterrupt();

    protected int SynchronizeWithInterrupt() => _state.SynchronizeWithInterrupt(); 

    //TODO: See details in IRegisterDP
    protected byte DP { get => _state.DP; set => _state.DP = value; }

    /// <summary>
    /// Program Counter
    /// </summary>
    protected ushort PC { get => _state.PC; set => _state.PC = value; }

    /// <summary>
    /// 16-bit register <c>A.B</c>
    /// </summary>
    protected ushort D { get => _state.D; set => _state.D = value; }

    /// <summary>
    /// 8-bit register <c>A</c>
    /// </summary>
    protected byte A { get => _state.A(); set => _state.A(value); }

    /// <summary>
    /// 8-bit register <c>B</c>
    /// </summary>
    protected byte B { get => _state.B(); set => _state.B(value); }

    /// <summary>
    /// 16-bit register
    /// </summary>
    protected ushort X { get => _state.X; set => _state.X = value; }

    /// <summary>
    /// 16-bit register
    /// </summary>
    protected ushort Y { get => _state.Y; set => _state.Y = value; }

    /// <summary>
    /// 16-bit <c>STACK</c> register
    /// </summary>
    protected ushort S { get => _state.S; set => _state.S = value; }

    /// <summary>
    /// 16-bit <c>USER-STACK</c> register
    /// </summary>
    protected ushort U { get => _state.U; set => _state.U = value; }

    /// <summary>
    /// Condition Codes Register
    /// </summary>
    protected byte CC { get => _state.CC; set => _state.CC = value; }

    /// <summary>
    /// Condition Code Carry Flag
    /// </summary>
    protected bool CC_C { get => CC.BitC(); set => CC = CC.BitC(value); }

    /// <summary>
    /// Condition Code Entire Register State Stacked Flag
    /// </summary>
    protected bool CC_E { get => CC.BitE(); set => CC = CC.BitE(value); }

    /// <summary>
    /// Condition Code FIRQ Flag
    /// </summary>
    protected bool CC_F { get => CC.BitF(); set => CC = CC.BitF(value); }

    /// <summary>
    /// Condition Code Half-Carry Flag
    /// </summary>
    protected bool CC_H { get => CC.BitH(); set => CC = CC.BitH(value); }

    /// <summary>
    /// Condition Code IRQ Flag
    /// </summary>
    protected bool CC_I { get => CC.BitI(); set => CC = CC.BitI(value); }

    /// <summary>
    /// Condition Code Negative Flag
    /// </summary>
    protected bool CC_N { get => CC.BitN(); set => CC = CC.BitN(value); }

    /// <summary>
    /// Condition Code Overflow Flag
    /// </summary>
    protected bool CC_V { get => CC.BitV(); set => CC = CC.BitV(value); }

    /// <summary>
    /// Condition Code Zero Flag
    /// </summary>
    protected bool CC_Z { get => CC.BitZ(); set => CC = CC.BitZ(value); }

    /// <summary>
    /// Handles the intricacies of calculating the sum of two values: <c>a+b</c>
    /// </summary>
    /// <param name="a">first 8-bit</param>
    /// <param name="b">second 8-bit</param>
    /// <returns>object with summation results</returns>
    protected static IFunction Add(byte a, byte b, bool cc = false) => new Addition(a, b, cc.ToBit());

    /// <summary>
    /// Handles the intricacies of calculating the sum of two values: <c>a+b</c>
    /// </summary>
    /// <param name="a">first 16-bit</param>
    /// <param name="b">second 16-bit</param>
    /// <returns>object with summation results</returns>
    protected static IFunction Add(ushort a, ushort b, bool cc = false) => new Addition(a, b, cc.ToBit());

    /// <summary>
    /// Handles the intricacies of calculating the difference of two values: <c>a-b</c>
    /// </summary>
    /// <param name="a">first 16-bit</param>
    /// <param name="b">second 16-bit</param>
    /// <returns>object with summation results</returns>
    protected static IFunction Subtract(byte a, byte b, bool cc = false) => new Subtraction(a, b, cc.ToBit());

    /// <summary>
    /// Handles the intricacies of calculating the difference of two values: <c>a-b</c>
    /// </summary>
    /// <param name="a">first 16-bit</param>
    /// <param name="b">second 16-bit</param>
    /// <returns>object with summation results</returns>
    protected static IFunction Subtract(ushort a, ushort b, bool cc = false) => new Subtraction(a, b, cc.ToBit());

    /// <summary>
    /// Handles the intricacies of calculating the product of two values: <c>a*b</c>
    /// </summary>
    /// <param name="a">first 16-bit</param>
    /// <param name="b">second 16-bit</param>
    /// <returns>object with summation results</returns>
    protected static IFunction Multiply(byte a, byte b) => new Multiplication(a, b);

    protected void Push(byte value) { M8[--S] = value; }

    protected void Push(ushort value) { Push(value.L()); Push(value.H()); }

    protected byte Pop8() { return M8[S++]; }

    protected ushort Pop16() { return (ushort)(Pop8() << 8 | Pop8()); }
}
