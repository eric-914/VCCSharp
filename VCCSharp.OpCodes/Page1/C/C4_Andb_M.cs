using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>C4/ANDB/IMMEDIATE</code>
/// Logically AND Memory Byte with Accumulator <c>B</c>
/// <code>B’ ← B AND (M)</code>
/// </summary>
/// <remarks>
/// The <c>ANDB</c> instruction logically ANDs the contents of a byte in memory with Accumulator <c>B</c>. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0  ]
/// 
/// The 8-bit result is then placed in the specified accumulator.
///     N The Negative flag is set equal to the new value of bit 7 of the accumulator.
///     Z The Zero flag is set if the new value of the accumulator is zero; cleared otherwise.
///     V The Overflow flag is cleared by this instruction.
///     
/// The AND instructions are commonly used for clearing bits and for testing bits. 
/// 
/// Consider the following examples:
///     ANDA #%11101111 ;Clears bit 4 in A
///     ANDA #%00000100 ;Sets Z flag if bit 2 is not set
/// 
/// When testing bits, it is often preferable to use the BIT instructions instead, since they perform the same logical AND operation without modifying the contents of the accumulator.    
/// 
/// Cycles (2)
/// Byte Count (2)
/// 
/// See Also: AIM, ANDCC, ANDD, ANDR, BAND, BIAND, BIT
internal class _C4_Andb_M : OpCode, IOpCode
{
    internal _C4_Andb_M(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        byte value = M8[PC++];

        byte result = (byte)(B & value);

        CC_N = B.Bit7();
        CC_Z = B == 0;
        CC_V = false;

        B = result;

        return 2;
    }
}
