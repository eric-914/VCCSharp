using VCCSharp.OpCodes.HD6309;
using VCCSharp.OpCodes.Model.Functions;
using VCCSharp.OpCodes.Model.Memory;
using VCCSharp.OpCodes.Model.Support;
using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.Model.OpCodes;

/// <summary>
/// For use on HD6309 specific opcodes
/// <code>🚫 6309 ONLY 🚫</code>
/// </summary>
internal abstract class OpCode6309
{
    public ISystemState _system { get; set; } = default!;

    private IState _state => _system.State;

    /// <summary>
    /// 8-bit memory access
    /// </summary>
    protected Memory8Bit M8 => _system.M8;

    /// <summary>
    /// 16-bit memory access
    /// </summary>
    protected Memory16Bit M16 => _system.M16;

    /// <summary>
    /// 32-bit memory access
    /// </summary>
    protected Memory32Bit M32 => _system.M32;

    protected MemoryDirect DIRECT => _system.DIRECT;

    /// <summary>
    /// 8-bit "Effective Address" memory access
    /// </summary>
    protected MemoryIndexed INDEXED => _system.INDEXED;

    /// <summary>
    /// Index accessor for 8-bit registers
    /// </summary>
    public IRegisters8Bit R8 => _system.R8;

    /// <summary>
    /// Index accessor for 16-bit registers
    /// </summary>
    public IRegisters16Bit R16 => _system.R16;

    protected Exceptions Exceptions => _system.Exceptions;

    protected DynamicCycles DynamicCycles => new DynamicCycles(() => _system.Mode);

    /// <summary>
    /// For use on EA/INDEXED memory access as EA has cycle penalties.
    /// </summary>
    public int Cycles { get => _system.Cycles; set => _system.Cycles = value; }

    protected void EndInterrupt() => _state.ClearInterrupt();

    protected int SyncWait() => _state.SynchronizeWithInterrupt();

    //TODO: See details in IRegisterDP
    public byte DP { get => _state.DP; set => _state.DP = value; }

    /// <summary>
    /// Program Counter
    /// </summary>
    protected ushort PC { get => _state.PC; set => _state.PC = value; }

    /// <summary>
    /// 16-bit register <c>A.B</c>
    /// </summary>
    public ushort D { get => _state.D; set => _state.D = value; }

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
    public byte CC { get => _state.CC; set => _state.CC = value; }

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
    /// 16-bit register <c>E.F</c>
    /// </summary>
    public ushort W { get => _state.W; set => _state.W = value; }

    /// <summary>
    /// 8-bit register
    /// </summary>
    protected byte E { get => _state.E(); set => _state.E(value); }

    /// <summary>
    /// 8-bit register
    /// </summary>
    protected byte F { get => _state.F(); set => _state.F(value); }

    /// <summary>
    /// 32-bit register <c>D.W</c> or <c>A.B.E.F</c>
    /// </summary>
    protected uint Q { get => _state.Q; set => _state.Q = value; }

    /// <summary>
    /// Mode Register
    /// </summary>
    protected byte MD { get => _state.MD; set => _state.MD = value; }

    /// <summary>
    /// <c>(NM)</c> Native Mode (reduced cycles, W stacked on interrupts)
    /// </summary>
    protected bool MD_NATIVE6309 { get => MD.Bit_NATIVE6309(); }

    /// <summary>
    /// <c>(FM)</c> FIRQ uses IRQ stacking method (Entire state)
    /// </summary>
    protected bool MD_FIRQMODE { get => MD.Bit_FIRQMODE(); }

    /// <summary>
    /// <c>(IL)</c> Illegal Instruction Exception
    /// </summary>
    protected bool MD_ILLEGAL { set => MD = MD.Bit_ILLEGAL(value); }

    /// <summary>
    /// <c>(/0)</c> Divide-by-zero Exception
    /// </summary>
    protected bool MD_ZERODIV { set => MD = MD = MD.Bit_ZERODIV(value); }

    /// <summary>
    /// Handles the intricacies of calculating the sum two values: <c>a+b</c>
    /// </summary>
    /// <param name="a">first 8-bit</param>
    /// <param name="b">second 8-bit</param>
    /// <returns>object with summation results</returns>
    protected static IFunction Add(byte a, byte b) => new Addition(a, b, 0);
    protected static IFunction Add(byte a, byte b, bool cc) => new Addition(a, b, cc.ToBit());

    /// <summary>
    /// Handles the intricacies of calculating the sum two values: <c>a+b</c>
    /// </summary>
    /// <param name="a">first 16-bit</param>
    /// <param name="b">second 16-bit</param>
    /// <returns>object with summation results</returns>
    protected static IFunction Add(ushort a, ushort b) => new Addition(a, b, 0);
    protected static IFunction Add(ushort a, ushort b, bool cc) => new Addition(a, b, cc.ToBit());

    /// <summary>
    /// Handles the intricacies of calculating the difference two values: <c>a-b</c>
    /// </summary>
    /// <param name="a">first 16-bit</param>
    /// <param name="b">second 16-bit</param>
    /// <returns>object with summation results</returns>
    protected static IFunction Subtract(byte a, byte b) => new Subtraction(a, b, 0);
    protected static IFunction Subtract(byte a, byte b, bool cc) => new Subtraction(a, b, cc.ToBit());

    /// <summary>
    /// Handles the intricacies of calculating the difference two values: <c>a-b</c>
    /// </summary>
    /// <param name="a">first 16-bit</param>
    /// <param name="b">second 16-bit</param>
    /// <returns>object with summation results</returns>
    protected static IFunction Subtract(ushort a, ushort b) => new Subtraction(a, b, 0);
    protected static IFunction Subtract(ushort a, ushort b, bool cc) => new Subtraction(a, b, cc.ToBit());

    /// <summary>
    /// Handles the intricacies of calculating the product of two values: <c>a*b</c>
    /// </summary>
    /// <param name="a">first 16-bit</param>
    /// <param name="b">second 16-bit</param>
    /// <returns>object with summation results</returns>
    protected static IFunction Multiply(byte a, byte b) => new Multiplication(a, b);

    /// <summary>
    /// Handles the intricacies of calculating the product of two values: <c>a*b</c>
    /// </summary>
    /// <param name="a">first 16-bit</param>
    /// <param name="b">second 16-bit</param>
    /// <returns>object with summation results</returns>
    protected static IFunction Multiply(ushort a, ushort b) => new Multiplication(a, b);

    protected static IFunctionDiv Divide(short numerator, sbyte denominator, int cycles) => new Division(numerator, denominator, cycles);
    protected static IFunctionDiv Divide(int numerator, short denominator, int cycles) => new Division(numerator, denominator, cycles);

    protected static IFunction Boolean(byte result) => new Functions.Boolean(result);
    protected static IFunction Boolean(ushort result) => new Functions.Boolean(result);
}
