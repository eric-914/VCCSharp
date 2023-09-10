using VCCSharp.OpCodes.Model.Support;

namespace VCCSharp.OpCodes.Model.Functions;

/// <summary>
/// Handles the intracies of adding two values: <c>a+b</c>
/// </summary>
internal class Addition : IFunction
{
    public int Result { get; private set; }
    public int Remainder => throw new NotImplementedException();
    public int Cycles => throw new NotImplementedException();

    /// <summary>
    /// <c>H</c>: The Half-Carry flag is set if a carry out of bit 3/7 (into bit 4/8) occurred; cleared otherwise.
    /// </summary>
    public bool H { get; }
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

    public bool Error => throw new NotImplementedException();

    public Addition(byte a, byte b, byte cc)
    {
        H = (a & 0x0F) + (b & 0x0F) + cc > 0x0F;
        Exec(a, b, cc, 0xFF, x => x.Bit7());
    }

    public Addition(ushort a, ushort b, byte cc)
    {
        H = false; //--Not applicable
        Exec(a, b, cc, 0xFFFF, x => x.Bit15());
    }

    public void Exec(int a, int b, int cc, int max, Func<int, bool> bit)
    {
        Result = a + b + cc;

        N = bit(Result);
        Z = (Result & max) == 0;
        V = bit((a & b & Result.I()) | (a.I() & b.I() & Result));
        C = Result > max;
    }
}
