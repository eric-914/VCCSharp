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

    /// <summary>
    /// 8-bit "Effective Address" memory access
    /// </summary>
    protected MemoryIndexed INDEXED { get; }

    /// <summary>
    /// Index accessor for 8-bit registers
    /// </summary>
    public IRegisters8Bit R8 { get;}

    /// <summary>
    /// Index accessor for 16-bit registers
    /// </summary>
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
    protected byte A { get => _cpu.A; set => _cpu.A = value; }

    /// <summary>
    /// 8-bit register
    /// </summary>
    protected byte B { get => _cpu.B; set => _cpu.B = value; }

    /// <summary>
    /// 8-bit register
    /// </summary>
    protected byte E { get => _cpu.E; set => _cpu.E = value; }

    /// <summary>
    /// 8-bit register
    /// </summary>
    protected byte F { get => _cpu.F; set => _cpu.F = value; }

    //TODO: See details in IRegisterDP
    public byte DP { get => _cpu.DP; set => _cpu.DP = value; }

    /// <summary>
    /// Program Counter
    /// </summary>
    protected ushort PC { get => _cpu.PC; set => _cpu.PC = value; }

    /// <summary>
    /// 16-bit register <c>A.B</c>
    /// </summary>
    public ushort D { get => _cpu.D; set => _cpu.D = value; }

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
    public byte CC { get => _cpu.CC; set => _cpu.CC = value; }

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
    public ushort W { get => _cpu.W; set => _cpu.W = value; }

    /// <summary>
    /// 32-bit register <c>D.W</c> or <c>A.B.E.F</c>
    /// </summary>
    protected uint Q { get => _cpu.Q; set => _cpu.Q = value; }

    /// <summary>
    /// Mode Register
    /// </summary>
    protected byte MD { get => _cpu.MD; set => _cpu.MD = value; }

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
    protected bool MD_ZERODIV { set => MD = MD = MD.Bit_ILLEGAL(value); }

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
