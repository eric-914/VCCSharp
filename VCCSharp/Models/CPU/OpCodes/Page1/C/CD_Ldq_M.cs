using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1.C
{
    /// <summary>
    /// LDQ
    /// 🚫 6309 ONLY 🚫
    /// Load 32-bit Data into Accumulator Q
    /// IMMEDIATE
    /// Q’ ← IMM32|(M:M+3)
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// LDQ             IMMEDIATE           CD          5           5 
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ 0  ]
    /// </summary>
    /// <remarks>
    /// This instruction loads either a 32-bit immediate value or the contents of a quad-byte value from memory (in big-endian order) into the Q accumulator. 
    /// The Condition Codes are affected as follows.
    ///         N The Negative flag is set equal to the new value of bit 31 of Accumulator Q.
    ///         Z The Zero flag is set if the new value of Accumulator Q is zero; cleared otherwise.
    ///         V The Overflow flag is always cleared.
    ///         C The Carry flag is not affected by this instruction.
    ///         
    /// See Also: LD (8-bit), LD (16-bit)
    /// </remarks>
    public class CD_Ldq_M : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            cpu.Q_REG = cpu.MemRead32(cpu.PC_REG);
            cpu.PC_REG += 4;

            cpu.CC_Z = ZTEST(cpu.Q_REG);
            cpu.CC_N = NTEST32(cpu.Q_REG);
            cpu.CC_V = false;

            return 5;
        }
    }
}
