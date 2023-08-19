using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// STX
    /// Store D accumulator to memory
    /// Store 16-Bit Register to Memory
    /// DIRECT
    /// (M:M+1)’ ← r
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// STX             DIRECT              9F          5 / 4       2 
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ 0  ]
    /// </summary>
    /// <remarks>
    /// These instructions store the contents of one of the 16-bit accumulators (D,W) or one of the 16-bit Index/Stack registers (X,Y,U,S) to a pair of memory bytes in big-endian order.
    /// The Condition Codes are affected as follows:
    ///         N The Negative flag is set equal to the value in bit 15 of the register.
    ///         Z The Zero flag is set if the register value is zero; cleared otherwise.
    ///         V The Overflow flag is always cleared.
    ///         C The Carry flag is not affected by these instructions.
    ///         
    /// See Also: ST (8-bit), STQ
    /// </remarks>
    public class _9F_Stx_D : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);

            cpu.MemWrite16(cpu.X_REG, address);

            cpu.CC_Z = ZTEST(cpu.X_REG);
            cpu.CC_N = NTEST16(cpu.X_REG);
            cpu.CC_V = false;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 5);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._54);
    }
}
