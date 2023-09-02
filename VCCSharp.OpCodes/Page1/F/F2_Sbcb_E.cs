using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>F2/SBCA/EXTENDED</code>
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
/// Cycles (5 / 4)
/// Byte Count (3)
/// 
/// See Also: SBCD, SBCR
internal class _F2_Sbcb_E : OpCode, IOpCode
{
    public int Exec()
    {
        ushort address = M16[PC+=2];
        byte value = M8[address];

        var sum = Subtract(B, value);

        //CC_H = undefined;
        CC_N = sum.N;
        CC_Z = sum.Z;
        CC_V = sum.V;
        CC_C = sum.C;

        B = (byte)sum.Result;

        return DynamicCycles._54;
    }
}
