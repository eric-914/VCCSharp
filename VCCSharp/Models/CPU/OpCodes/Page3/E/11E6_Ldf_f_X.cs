using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3.E
{
    /// <summary>
    /// LDF
    /// 🚫 6309 ONLY 🚫
    /// Load accumulator from memory
    /// Load Data into 8-Bit Accumulator
    /// INDEXED
    /// r’ ← IMM8|(M)
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// LDF             INDEXED             11E6        5+          3+ 
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
    public class _11E6_Ldf_X : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.INDADDRESS(cpu.PC_REG++);

            cpu.F_REG = cpu.MemRead8(address);

            cpu.CC_Z = ZTEST(cpu.F_REG);
            cpu.CC_N = NTEST8(cpu.F_REG);
            cpu.CC_V = false;

            return 5;
        }
    }
}
