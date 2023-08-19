using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2.D
{
    /// <summary>
    /// STQ
    /// 🚫 6309 ONLY 🚫
    /// Store Contents of Accumulator Q to Memory
    /// DIRECT
    /// (M:M+3)’ ← Q 
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// STQ             DIRECT              10DD        8 / 7       3 
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ 0  ]
    /// </summary>
    /// <remarks>
    /// This instruction stores the contents of the Q accumulator into 4 sequential bytes of memory in big-endian order. 
    /// The Condition Codes are affected as follows.
    ///         N The Negative flag is set equal to the value of bit 31 of Accumulator Q.
    ///         Z The Zero flag is set if the value of Accumulator Q is zero; cleared otherwise.
    ///         V The Overflow flag is always cleared.
    ///         C The Carry flag is not affected by this instruction.
    ///         
    /// See Also: ST (8-bit), ST (16-bit)
    /// </remarks>
    public class _10DD_Stq_D : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);

            cpu.MemWrite32(cpu.Q_REG, address);

            cpu.CC_Z = ZTEST(cpu.Q_REG);
            cpu.CC_N = NTEST32(cpu.Q_REG);
            cpu.CC_V = false;

            return Cycles._87;
        }
    }
}
