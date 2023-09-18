using VCCSharp.OpCodes.Definitions;
using VCCSharp.OpCodes.Model.Support;

namespace VCCSharp.OpCodes.Model.Functions;

/// <summary>
/// Division is more complex than the other functions as it has a remainder, its cycle execution time can vary, and can 'throws' an exception.
/// </summary>
internal class Division : IFunctionDiv
{
    #region Properties

    public int Result { get; private set; }
    public int Remainder { get; private set; }
    public int Cycles { get; private set; }

    public bool E => throw new NotImplementedException();
    public bool F => throw new NotImplementedException();
    public bool H => throw new NotImplementedException();
    public bool I => throw new NotImplementedException();

    /// <summary>
    /// <c>N</c> The Negative flag is set equal to the new value of bit/15 7 in Accumulator B/W.
    /// </summary>
    public bool N { get; private set; }

    /// <summary>
    /// <c>Z</c> The Zero flag is set if the new value of Accumulator B/W is zero; cleared otherwise.
    /// </summary>
    public bool Z { get; private set; }

    /// <summary>
    /// <c>V</c> The Overflow flag is set if an overflow occurred; cleared otherwise.
    /// </summary>
    public bool V { get; private set; }

    /// <summary>
    /// <c>C</c> The Carry flag is set if the quotient in Accumulator B/W is odd; cleared if even.
    /// </summary>
    public bool C { get; private set; }

    /// <summary>
    /// Set when Divide By Zero occurs.  Error to be handled by client.
    /// </summary>
    public DivisionErrors Error { get; private set; } = DivisionErrors.None;

    #endregion

    public Division(short numerator, sbyte denominator, int cycles)
    {
        const byte abort = 0xFF;
        const byte max = 0x7F;

        Cycles = cycles;

        if (denominator == 0)
        {
            Error = DivisionErrors.DivideByZero;

            return;
        }

        short result = (short)(numerator / denominator);

        //--range overflow
        if (result > abort || result < ~abort)
        {
            Error = DivisionErrors.RangeOverflow;

            N = false;
            Z = false;
            V = true;
            C = false;

            Cycles -= 13; //If a range overflow occurs, DIVD uses 13 fewer cycles than what is shown in the table.

            return;
        }

        Result = result;
        Remainder = (byte)(numerator % denominator);

        //--two’s complement overflow
        bool overflow = result > max || result < ~max;
        byte b = (byte)result;

        N = overflow || b.Bit7();
        Z = !overflow && b == 0;
        V = overflow;
        C = (b & 1) != 0;

        Cycles -= overflow.ToBit();
    }

    public Division(int numerator, short denominator, int cycles)
    {
        const ushort abort = 0xFFFF;
        const ushort max = 0x7FFF;

        Cycles = cycles;

        if (denominator == 0)
        {
            Error = DivisionErrors.DivideByZero;

            return;
        }

        int result = numerator / denominator;

        //--range overflow
        if (result > abort || result < ~abort)
        {
            Error = DivisionErrors.RangeOverflow;

            N = false;
            Z = false;
            V = true;
            C = false;

            Cycles -= 21; //If a range overflow occurs, DIVQ uses 21 fewer cycles than what is shown in the table.

            return;
        }

        int remainder = numerator % denominator;

        Result = result;
        Remainder = (ushort)remainder;

        //--two’s complement overflow
        bool overflow = result > max || result < ~max;
        ushort w = (ushort)result;
        byte b = (byte)remainder;

        N = overflow || w.Bit15();
        Z = !overflow && w == 0;
        V = overflow;
        C = (b & 1) != 0;

        Cycles -= overflow.ToBit();
    }
}
