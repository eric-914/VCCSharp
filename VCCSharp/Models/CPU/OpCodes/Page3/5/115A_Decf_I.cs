using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3
{
    /// <summary>
    /// DECF
    /// 🚫 6309 ONLY 🚫
    /// Decrement accumulator or memory location
    /// Decrement Accumulator
    /// INHERENT
    /// r’ ← r - 1
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// DECF            INHERENT            115A        3 / 2       2
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ ↕  ]
    /// </summary>
    /// <remarks>
    /// These instructions subtract 1 from the specified accumulator. 
    /// The Condition Code flags are affected as follows:
    ///         N The Negative flag is set equal to the new value of the accumulators high-order bit.
    ///         Z The Zero flag is set if the new value of the accumulator is zero; cleared otherwise.
    ///         V The Overflow flag is set if the original value was 80(16) (8-bit) or 8000(16) (16-bit); cleared otherwise.
    ///         C The Carry flag is not affected by these instructions.
    ///         
    /// It is important to note that the DEC instructions do not affect the Carry flag. 
    /// This means that it is not always possible to optimize code by simply replacing a SUBr #1 instruction with a corresponding DECr. 
    /// Because the DEC instructions do not affect the Carry flag, they can be used to implement loop counters within multiple precision computations.
    /// 
    /// When used to decrement an unsigned value, only the BEQ and BNE branches will always behave as expected. 
    /// When operating on a signed value, all of the signed conditional branch instructions will behave as expected.
    /// 
    /// See Also: DEC (memory), INC, SUB
    /// </remarks>
    public class _115A_Decf_I : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            cpu.F_REG--;

            cpu.CC_Z = ZTEST(cpu.F_REG);
            cpu.CC_N = NTEST8(cpu.F_REG);
            cpu.CC_V = cpu.F_REG == 0x7F;

            return Cycles._21;
        }
    }
}
