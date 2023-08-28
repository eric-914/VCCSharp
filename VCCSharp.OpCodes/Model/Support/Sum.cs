namespace VCCSharp.OpCodes.Model.Support;

/// <summary>
/// Handles the intracies of adding two values: <c>a+b</c>
/// </summary>
internal class Sum : IFunction
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

    private bool Carry(bool A, bool B, bool C) => (A && B) || (A && !C) || (B && !C);
    private bool Carry(byte a, byte b, byte c) => Carry(a.Bit7(), b.Bit7(), c.Bit7());
    private bool Carry(ushort a, ushort b, ushort c) => Carry(a.Bit15(), b.Bit15(), c.Bit15());

    private bool Overflow(bool A, bool B, bool C) => (A && B && !C) || (!A && !B && C);
    private bool Overflow(byte a, byte b, byte c) => Overflow(a.Bit7(), b.Bit7(), c.Bit7());
    private bool Overflow(ushort a, ushort b, ushort c) => Overflow(a.Bit15(), b.Bit15(), c.Bit15());

    private bool HalfCarry(byte a, byte b, byte c) => Carry(a.Bit3(), b.Bit3(), c.Bit3());

    public Sum(byte a, byte b)
    {
        byte c = (byte)(a + b);

        Result = c;

        H = HalfCarry(a, b, c);
        N = c.Bit7();
        Z = c == 0;
        V = Overflow(a, b, c);
        C = Carry(a, b, c);
    }

    public Sum(ushort a, ushort b)
    {
        ushort c = (ushort)(a + b);

        Result = c;

        H = false; //--Not applicable
        N = c.Bit15();
        Z = c == 0;
        V = Overflow(a, b, c);
        C = Carry(a, b, c);
    }
}
