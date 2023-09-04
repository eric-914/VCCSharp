using VCCSharp.OpCodes.Model.Support;

namespace VCCSharp.OpCodes.Model.Functions;

/// <summary>
/// Handles the intracies of adding two values: <c>a+b</c>
/// </summary>
internal class Subtraction : IFunction
{
    public int Result { get; }

    /// <summary>
    /// <c>H</c>: The Half-Carry flag is set if a carry out of bit 3/7 (into bit 4/8) occurred; cleared otherwise.
    /// </summary>
    public bool H { get; }
    /// <summary>
    /// <c>N</c>: The Negative flag is set equal to the new value of bit 7/15 of the accumulator.
    /// </summary>
    public bool N { get; }
    /// <summary>
    /// <c>Z</c>: The Zero flag is set if the new accumulator value is zero; cleared otherwise.
    /// </summary>
    public bool Z { get; }
    /// <summary>
    /// <c>V</c>: The Overflow flag is set if an overflow occurred; cleared otherwise.
    /// </summary>
    public bool V { get; }
    /// <summary>
    /// <c>C</c>: The Carry flag is set if a carry out of bit 7/15 (into bit 8/16) occurred; cleared otherwise.
    /// </summary>
    public bool C { get; }

    public bool I => throw new NotImplementedException();
    public bool F => throw new NotImplementedException();
    public bool E => throw new NotImplementedException();

    public Subtraction(byte a, byte b)
    {
        Result = a - b;

        H = (b & 0x0F) > (a & 0x0F);
        N = Result.Bit7();
        Z = Result == 0;
        V = ((a & b.I() & Result.I()) | (a.I() & b & Result)).Bit7();
        C = b > a;
    }

    public Subtraction(ushort a, ushort b)
    {
        Result = a - b;

        H = false; //--Not applicable
        N = Result.Bit15();
        Z = Result == 0;
        V = ((a & b & Result.I()) | (a.I() & b.I() & Result)).Bit15();
        C = b > a;
    }
}
