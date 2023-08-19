using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// STY
    /// Store D accumulator to memory
    /// Store 16-Bit Register to Memory
    /// DIRECT
    /// (M:M+1)’ ← r
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// STY             DIRECT              109F        6 / 5       3 
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
    public class _109F_Sty_D : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);

            cpu.MemWrite16(cpu.Y_REG, address);

            cpu.CC_Z = ZTEST(cpu.Y_REG);
            cpu.CC_N = NTEST16(cpu.Y_REG);
            cpu.CC_V = false; //0;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 7); //TODO: One of these is wrong?
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._65);
    }
}
