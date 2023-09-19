using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>D2/SBCA/IMMEDIATE</code>
/// Subtract Memory Byte and Carry (borrow) from Accumulator <c>B</c>
/// <code>B’ ← B - IMM8|(M) - C</code>
/// </summary>
/// <remarks>
/// The <c>SBCB</c> instruction subtracts the 8-bit immediate value and the value of the Carry flag from the <c>B</c> accumulator. 
/// The 8-bit result is placed back into the <c>B</c> accumulator. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [    ~   ↕ ↕ ↕ ↕]
///   
/// Note that since subtraction is performed, the purpose of the Carry flag is to represent a Borrow.
///         H The affect on the Half-Carry flag is undefined for these instructions.
///         N The Negative flag is set equal to the new value of bit 7 of the accumulator.
///         Z The Zero flag is set if the new accumulator value is zero; cleared otherwise.
///         V The Overflow flag is set if an overflow occurred; cleared otherwise.
///         C The Carry flag is set if a borrow into bit-7 was needed; cleared otherwise.
/// 
/// The SBC instruction is most often used to perform subtraction of the subsequent bytes of a multi-byte subtraction. 
/// This allows the borrow from a previous SUB or SBC instruction to be included when doing subtraction for the next higher-order byte.
/// Since the 6809 and 6309 both provide 16-bit SUB instructions for the accumulators, it is not necessary to use the 8-bit SUB and SBC instructions to perform 16-bit subtraction.
/// 
/// Cycles (4 / 3)
/// Byte Count (2)
///         
/// See Also: SBCD, SBCR
internal class _D2_Sbcb_D : OpCode, IOpCode
{
    public int Exec()
    {
        ushort address = DIRECT[PC++];
        byte value = M8[address];

        var fn = Subtract(B, value, CC_C);

        //CC_H = undefined;
        CC_N = fn.N;
        CC_Z = fn.Z;
        CC_V = fn.V;
        CC_C = fn.C;

        B = (byte)fn.Result;

        return DynamicCycles._43;
    }
}
