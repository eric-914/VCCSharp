using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1.D
{
    /// <summary>
    /// LDB
    /// Load accumulator from memory
    /// Load Data into 8-Bit Accumulator
    /// DIRECT
    /// r’ ← IMM8|(M)
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// LDB             DIRECT              D6          4 / 3       2 
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ 0  ]
    /// </summary>
    /// <remarks>
    /// These instructions load either an 8-bit immediate value or the contents of a memory byte into one of the 8-bit accumulators (A,B,E,F). 
    /// The Condition Codes are affected as follows.
    ///         N The Negative flag is set equal to the new value of bit 7 of the accumulator.
    ///         Z The Zero flag is set if the new accumulator value is zero; cleared otherwise.
    ///         V The Overflow flag is always cleared.
    ///         C The Carry flag is not affected by these instructions.
    /// 
    /// See Also: LD (16-bit), LDQ
    /// </remarks>
    public class D6_Ldb_D : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            var address = cpu.DPADDRESS(cpu.PC_REG++);

            cpu.B_REG = cpu.MemRead8(address);

            cpu.CC_Z = ZTEST(cpu.B_REG);
            cpu.CC_N = NTEST8(cpu.B_REG);
            cpu.CC_V = false;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 4);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._43);
    }
}
