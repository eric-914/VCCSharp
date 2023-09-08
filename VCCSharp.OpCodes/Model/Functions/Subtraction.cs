using VCCSharp.OpCodes.Model.Support;

namespace VCCSharp.OpCodes.Model.Functions;

/// <summary>
/// Handles the intracies of adding two values: <c>a+b</c>
/// </summary>
internal class Subtraction : IFunction
{
    public int Result { get; private set; }

    /// <summary>
    /// <c>H</c>: The Half-Carry flag is undefined for 8-bit subtraction
    /// </summary>
    public bool H => throw new NotImplementedException();
    /// <summary>
    /// <c>N</c>: The Negative flag is set equal to the new value of bit 7/15 of the accumulator.
    /// </summary>
    public bool N { get; private set; }
    /// <summary>
    /// <c>Z</c>: The Zero flag is set if the new accumulator value is zero; cleared otherwise.
    /// </summary>
    public bool Z { get; private set; }
    /// <summary>
    /// <c>V</c>: The Overflow flag is set if an overflow occurred; cleared otherwise.
    /// </summary>
    public bool V { get; private set; }
    /// <summary>
    /// <c>C</c>: The Carry flag is set if a carry out of bit 7/15 (into bit 8/16) occurred; cleared otherwise.
    /// </summary>
    public bool C { get; private set; }

    public bool I => throw new NotImplementedException();
    public bool F => throw new NotImplementedException();
    public bool E => throw new NotImplementedException();

    public Subtraction(byte a, byte b, byte cc)
    {
        Exec(a, b, cc, 0xFF, x => x.Bit7());
    }

    public Subtraction(ushort a, ushort b, byte cc)
    {
        Exec(a, b, cc, 0xFFFF, x => x.Bit15());
    }

    public void Exec(int a, int b, int cc, int max, Func<int, bool> bit)
    {
        Result = a - (b + cc);

        N = bit(Result);
        Z = (Result & max) == 0;
        V = bit(((a & b.I() & Result.I()) | (a.I() & b & Result)));
        C = (b + cc) > a;
    }
}
