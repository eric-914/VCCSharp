using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// NEGA
    /// Negate accumulator or memory
    /// Negation (Twos-Complement) of Accumulator
    /// INHERENT
    /// r’ ← 0 - r
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// NEGA            INHERENT            40          2 / 1       1
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
    public class _40_Nega_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            var _temp8 = (byte)(0 - cpu.A_REG);

            cpu.CC_C = _temp8 > 0;
            cpu.CC_V = cpu.A_REG == 0x80; //CC_C ^ ((A_REG^temp8)>>7);
            cpu.CC_N = NTEST8(_temp8);
            cpu.CC_Z = ZTEST(_temp8);

            cpu.A_REG = _temp8;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 2);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._21);
    }
}
