using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>11F1/CMPF/EXTENDED</code>
/// Compare Memory Byte from 8-Bit Accumulator <c>F</c>
/// <code>TEMP ← F - (M)</code>
/// </summary>
/// <remarks>
/// The <c>CMPF</c> instruction subtracts the contents of a byte in memory from the value in the 8-bit <c>F</c> accumulator and set the Condition Codes accordingly.
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [    ~   ↕ ↕ ↕ ↕]
///   
/// Neither the memory byte nor the accumulator are modified.
///         H The affect on the Half-Carry flag is undefined for these instructions.
///         N The Negative flag is set equal to the value of bit 7 of the result.
///         Z The Zero flag is set if the resulting value is zero; cleared otherwise.
///         V The Overflow flag is set if an overflow occurred; cleared otherwise.
///         C The Carry flag is set if a borrow into bit-7 was needed; cleared otherwise.
///         
/// The Compare instructions are usually used to set the Condition Code flags prior to executing a conditional branch instruction.
/// 
/// The 8-bit CMPE instruction performs exactly the same operation as the 8-bit SUBE instruction, with the exception that the value in the accumulator is not changed. 
/// Note that since a subtraction is performed, the Carry flag actually represents a Borrow.
/// 
/// Cycles (6 / 5)
/// Byte Count (4)
/// 
/// See Also: CMP (16-bit), CMPR
internal class _11F1_Cmpf_E : OpCode6309, IOpCode
{
    internal _11F1_Cmpf_E(HD6309.IState cpu) : base(cpu) { }

    public int Exec()
    {
        ushort address = M16[PC += 2];
        byte value = M8[address];

        var sum = Subtract(F, value);

        //CC_H = undefined
        CC_N = sum.N;
        CC_Z = sum.Z;
        CC_V = sum.V;
        CC_C = sum.C;

        return DynamicCycles._65;
    }
}
