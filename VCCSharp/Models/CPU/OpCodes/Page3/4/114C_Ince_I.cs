using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3
{
    /// <summary>
    /// INCE
    /// 🚫 6309 ONLY 🚫
    /// Increment accumulator or memory location
    /// Increment Accumulator
    /// INHERENT
    /// r’ ← r + 1
    /// SOURCE FORM         ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// INCE                INHERENT            114C        3 / 2       2
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ ↕  ]
    /// </summary>
    /// <remarks>
    /// These instructions add 1 to the contents of the specified accumulator. 
    /// The Condition Code flags are affected as follows:
    ///         N The Negative flag is set equal to the new value of the accumulators high-order bit.
    ///         Z The Zero flag is set if the new value of the accumulator is zero; cleared otherwise.
    ///         V The Overflow flag is set if the original value was $7F (8-bit) or $7FFF (16-bit); cleared otherwise.
    ///         C The Carry flag is not affected by these instructions.
    ///         
    /// It is important to note that the INC instructions do not affect the Carry flag. 
    /// This means that it is not always possible to optimize code by simply replacing an ADDr #1 instruction with a corresponding INCr.
    /// 
    /// When used to increment an unsigned value, only the BEQ and BNE branches will consistently behave as expected. 
    /// When operating on a signed value, all of the signed conditional branch instructions will behave as expected.
    /// 
    /// See Also: ADD, DEC, INC (memory)
    /// </remarks>
    public class _114C_Ince_I : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            cpu.E_REG++;

            cpu.CC_Z = ZTEST(cpu.E_REG);
            cpu.CC_N = NTEST8(cpu.E_REG);
            cpu.CC_V = cpu.E_REG == 0x80;

            return Cycles._32;
        }
    }
}
