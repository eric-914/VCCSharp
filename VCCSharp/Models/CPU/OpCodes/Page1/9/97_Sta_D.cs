using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// STA
    /// Store accumulator to memory
    /// Store 8-Bit Accumulator to Memory
    /// DIRECT
    /// (M)’ ← r
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// STA             DIRECT              97          4 / 3       2 
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ 0  ]
    /// </summary>
    /// <remarks>
    /// These instructions store the contents of one of the 8-bit accumulators (A,B,E,F) into a byte in memory. 
    /// The Condition Codes are affected as follows.
    ///         N The Negative flag is set equal to the value of bit 7 of the accumulator.
    ///         Z The Zero flag is set if the accumulator’s value is zero; cleared otherwise.
    ///         V The Overflow flag is always cleared.
    ///         C The Carry flag is not affected by these instructions.
    ///         
    /// See Also: ST (16-bit), STQ
    /// </remarks>
    public class _97_Sta_D : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);

            cpu.MemWrite8(cpu.A_REG, address);

            cpu.CC_Z = ZTEST(cpu.A_REG);
            cpu.CC_N = NTEST8(cpu.A_REG);
            cpu.CC_V = false;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 4);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._43);
    }
}
