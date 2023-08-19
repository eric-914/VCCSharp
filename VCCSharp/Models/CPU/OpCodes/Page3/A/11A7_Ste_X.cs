using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3.A
{
    /// <summary>
    /// STE
    /// 🚫 6309 ONLY 🚫
    /// Store accumulator to memory
    /// Store 8-Bit Accumulator to Memory
    /// INDEXED
    /// (M)’ ← r
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// STE             INDEXED             11A7        5+          3+ 
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
    public class _11A7_Ste_X : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.INDADDRESS(cpu.PC_REG++);

            cpu.MemWrite8(cpu.E_REG, address);

            cpu.CC_Z = ZTEST(cpu.E_REG);
            cpu.CC_N = NTEST8(cpu.E_REG);
            cpu.CC_V = false;

            return 5;
        }
    }
}
