using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1.F
{
    /// <summary>
    /// LDU
    /// Load stack pointer from memory
    /// Load Data into 16-Bit Register
    /// EXTENDED
    /// r’ ← IMM16|(M:M+1)
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// LDU             EXTENDED            FE          6 / 5       3
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ 0  ]
    /// </summary>
    /// <remarks>
    /// These instructions load either a 16-bit immediate value or the contents from a pair of memory bytes (in big-endian order) into one of the 16-bit accumulators (D,W) or one of the 16-bit Index registers (X,Y,U,S). 
    /// The Condition Codes are affected as follows.
    ///         N The Negative flag is set equal to the new value of bit 15 of the register.
    ///         Z The Zero flag is set if the new register value is zero; cleared otherwise.
    ///         V The Overflow flag is always cleared.
    ///         C The Carry flag is not affected by these instructions.
    ///         
    /// See Also: LD (8-bit), LDQ, LEA
    /// </remarks>
    public class FE_Ldu_E : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.MemRead16(cpu.PC_REG);

            cpu.U_REG = cpu.MemRead16(address);

            cpu.CC_Z = ZTEST(cpu.U_REG);
            cpu.CC_N = NTEST16(cpu.U_REG);
            cpu.CC_V = false;

            cpu.PC_REG += 2;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 6);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._65);
    }
}
