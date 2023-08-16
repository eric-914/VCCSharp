using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// DAA
    /// Decimal adjust A accumulator
    /// Decimal Addition Adjust
    /// INHERENT
    /// A[4..7]’ ← A[4..7] + 6 IF:
    ///     CC.C = 1
    ///     OR: A[4..7] > 9
    ///     OR: A[4..7] > 8 AND A[0..3] > 9
    /// A[0..3]’ ← A[0..3] + 6 IF:
    ///     CC.H = 1
    ///     OR: A[0..3] > 9
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// DAA             INHERENT            19          2 / 1       1
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ ~ ↕]
    /// </summary>
    /// <remarks>
    /// The DAA instruction is used after performing an 8-bit addition of Binary Coded Decimal values using either the ADDA or ADCA instructions. 
    /// DAA adjusts the value resulting from the binary addition in accumulator A so that it contains the desired BCD result instead. 
    /// The Carry flag is also updated to properly reflect BCD addition. 
    /// That is, the Carry flag is set when addition of the most-significant digits (plus any carry from the addition of the least-significant digits) produces a value greater than 9.
    ///         H The Half-Carry flag is not affected by this instruction.
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
    /// See Also: ADCA, ADDA
    /// </remarks>
    public class _19_Daa_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            byte msn = (byte)(cpu.A_REG & 0xF0);
            byte lsn = (byte)(cpu.A_REG & 0xF);

            byte mask = 0;

            if (cpu.CC_H || lsn > 9)
            {
                mask |= 0x06;
            }

            if (msn > 0x80 && lsn > 9)
            {
                mask |= 0x60;
            }

            if (msn > 0x90 || cpu.CC_C)
            {
                mask |= 0x60;
            }

            ushort value = (ushort)(cpu.A_REG + mask);

            cpu.CC_C |= (value & 0x100) >> 8 != 0;
            cpu.A_REG = (byte)value;
            cpu.CC_N = NTEST8(cpu.A_REG);
            cpu.CC_Z = ZTEST(cpu.A_REG);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 2);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._21);
    }
}
