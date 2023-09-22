using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>19/DAA/INHERENT</code>
/// Decimal Addition Adjust - A accumulator
/// <code>
/// IF (CC.C = 1) OR (A[4..7] > 9) OR (A[4..7] > 8 AND A[0..3] > 9) 
///     THEN: A[4..7]’ ← A[4..7] + 6 
/// IF (CC.H = 1) OR (A[0..3] > 9) 
///     THEN: A[0..3]’ ← A[0..3] + 6 
/// </code>
/// </summary>
/// <remarks>
/// The DAA instruction is used after performing an 8-bit addition of Binary Coded Decimal values using either the ADDA or ADCA instructions. 
/// DAA adjusts the value resulting from the binary addition in accumulator A so that it contains the desired BCD result instead. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ ~ ↕]
/// 
/// The DAA instruction is used after performing an 8-bit addition of Binary Coded Decimal values using either the ADDA or ADCA instructions. 
/// DAA adjusts the value resulting from the binary addition in accumulator A so that it contains the desired BCD result instead. 
/// The Carry flag is also updated to properly reflect BCD addition. 
/// That is, the Carry flag is set when addition of the most-significant digits (plus any carry from the addition of the least-significant digits) produces a value greater than 9.
///         N The Negative flag is set equal to the new value of bit 7 in Accumulator A.
///         Z The Zero flag is set if the new value of Accumulator A is zero; cleared otherwise.
///         V The affect this instruction has on the Overflow flag is undefined.
///         C The Carry flag is set if the BCD addition produced a carry; cleared otherwise.
///         
/// The code below adds the BCD values of 64 and 27, producing the BCD sum of 91:
///         LDA #$64
///         ADDA #$27   ; Produces binary result of $8B
///         DAA         ; Adjusts A to $91 (BCD result of 64 + 27)
///         
/// DAA is the only instruction which is affected by the value of the Half Carry flag (H) in the Condition Codes register.
/// 
/// Cycles (2 / 1)
/// Byte Count (1)
/// 
/// See Also: ADCA, ADDA
internal class _19_Daa_I : OpCode, IOpCode
{
    public int CycleCount => DynamicCycles._21;

    public void Exec()
    {
        byte msn = (byte)(A >> 4);  // -- A[4..7]
        byte lsn = (byte)(A & 0x0F); // -- A[0..3]

        ushort result = A;

        // IF (CC.C = 1) OR (A[4..7] > 9) OR (A[4..7] > 8 AND A[0..3] > 9) 
        if (CC_C || (msn > 9) || (msn > 8 && lsn > 9))
        {
            result += 0x60;  // THEN: A[4..7]’ ← A[4..7] + 6 
        }

        // IF (CC.H = 1) OR (A[0..3] > 9) 
        if (CC_H || lsn > 9)
        {
            result += 0x06;  // THEN: A[0..3]’ ← A[0..3] + 6 
        }

        A = (byte)result;

        CC_N = A.Bit7();
        CC_C |= result > 0xFF;
        //CC_V = undefined
        CC_Z = A == 0;
    }
}
