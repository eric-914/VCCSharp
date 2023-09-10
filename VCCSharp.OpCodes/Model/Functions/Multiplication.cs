using VCCSharp.OpCodes.Definitions;
using VCCSharp.OpCodes.Model.Support;

namespace VCCSharp.OpCodes.Model.Functions;

internal class Multiplication : IFunction
{
    public int Result { get; private set; }
    public int Remainder => throw new NotImplementedException();
    public int Cycles => throw new NotImplementedException();

    public bool E => throw new NotImplementedException();
    public bool F => throw new NotImplementedException();
    public bool H => throw new NotImplementedException();
    public bool I => throw new NotImplementedException();
    public bool V => throw new NotImplementedException();

    /// <summary>
    /// <c>N</c> The Negative flag is set if the twos complement result is negative; cleared otherwise.
    /// Applies to 16-bit × 16-bit only
    /// </summary>
    public bool N { get; private set; }

    /// <summary>
    /// <c>Z</c> The Zero flag is set if the result is zero; cleared otherwise.
    /// </summary>
    public bool Z { get; private set; }

    /// <summary>
    /// <c>C</c> The Carry flag is set equal to the new value of bit 7 in Accumulator B.
    /// </summary>
    /// <remarks>
    /// The Carry flag is set equal to bit 7 of the least-significant byte so that rounding of the most-significant byte can be accomplished by executing:
    /// <code>ADCA #0</code>
    /// Applies to 8-bit × 8-bit only
    /// </remarks>
    public bool C { get; private set; }

    public DivisionErrors Error => throw new NotImplementedException();

    public Multiplication(byte a, byte b)
    {
        Result = a * b;

        Z = Result == 0;
        C = Result.Bit7(); //--Weird
    }

    public Multiplication(ushort a, ushort b)
    {
        Result = (short)a * (short)b;

        N = Result.Bit31();
        Z = Result == 0;
    }
}
