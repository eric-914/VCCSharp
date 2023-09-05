using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>94/ANDA/DIRECT</code>
/// Logically AND Memory Byte with Accumulator <c>A</c>
/// <code>A’ ← A AND (M)</code>
/// </summary>
/// <remarks>
/// The <c>ANDA</c> instruction logically ANDs the contents of a byte in memory with Accumulator <c>A</c>. 
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
/// Cycles (4 / 3)
/// Byte Count (2)
/// 
/// See Also: AIM, ANDCC, ANDD, ANDR, BAND, BIAND, BIT
internal class _94_Anda_D : OpCode, IOpCode
{
    public int Exec()
    {
        ushort address = DIRECT[PC++];
        byte value = M8[address];
        
        byte result = (byte)(A & value);

        CC_N = result.Bit7();
        CC_Z = result == 0;
        CC_V = false;

        A = result;

        return DynamicCycles._43;
    }
}
