using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// NEGB
    /// Negate accumulator or memory
    /// Negation (Twos-Complement) of Accumulator
    /// INHERENT
    /// r’ ← 0 - r
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// NEGB            INHERENT            50          2 / 1       1
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ ↕ ↕]
    /// </summary>
    /// <remarks>
    /// Each of these instructions change the value of the specified accumulator to that of it’s twos-complement; that is the value which when added to the original value produces a sum of zero. 
    /// The Condition Code flags are also modified as follows:
    ///         N The Negative flag is set equal to the new value of the accumulators high-order bit.
    ///         Z The Zero flag is set if the new value of the accumulator is zero; cleared otherwise.
    ///         V The Overflow flag is set if the original value was 80(16) (8-bit) or 8000(16) (16-bit); cleared otherwise.
    ///         C The Carry flag is cleared if the original value was 0; set otherwise.
    /// 
    /// The operation performed by the NEG instruction can be expressed as:
    ///         result = 0 - value
    ///         
    /// The Carry flag represents a Borrow for this operation and is therefore always set unless the accumulator’s original value was zero.
    /// 
    /// If the original value of the accumulator is 80(16) (8000(16) for NEGD) then the Overflow flag (V) is set and the accumulator’s value is not modified.
    /// 
    /// This instruction performs a twos-complement operation. 
    /// A ones-complement can be achieved with the COM instruction.
    /// 
    /// The 6309 does not provide instructions for negating the E, F, W and Q accumulators. 
    /// A 32-bit negation of Q can be achieved with the following instructions:
    ///         COMD
    ///         COMW
    ///         ADCR 0,W
    ///         ADCR 0,D
    /// 
    /// See Also: COM, NEG (memory)
    /// </remarks>
    public class _50_Negb_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            byte negative = (byte)(0 - cpu.B_REG);

            cpu.CC_C = negative > 0;
            cpu.CC_V = cpu.B_REG == 0x80;
            cpu.CC_N = NTEST8(negative);
            cpu.CC_Z = ZTEST(negative);

            cpu.B_REG = negative;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 2);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._21);
    }
}
