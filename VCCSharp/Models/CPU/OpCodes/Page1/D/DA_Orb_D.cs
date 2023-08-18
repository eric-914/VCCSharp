using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1.D
{
    /// <summary>
    /// ORB
    /// Or memory with accumulator
    /// Logically OR Accumulator with a Byte from Memory
    /// DIRECT
    /// r’ ← r OR IMM8|(M)
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// ORB             DIRECT              DA          4 / 3       2 
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ 0  ]
    /// </summary>
    /// <remarks>
    /// These instructions logically OR the contents of Accumulator A or B with either an 8-bit immediate value or the contents of a memory byte. 
    /// The 8-bit result is then placed back in the specified accumulator.
    ///         N The Negative flag is set equal to the new value of bit 7 of the accumulator.
    ///         Z The Zero flag is set if the new value of the accumulator is zero; cleared otherwise.
    ///         V The Overflow flag is cleared by this instruction.
    ///         C The Carry flag is not affected by this instruction.
    ///         
    /// The OR instructions are commonly used for setting specific bits in an accumulator to '1' while leaving other bits unchanged. 
    /// Consider the following examples:
    ///         ORA #%00010000  ;Sets bit 4 in A
    ///         ORB #$7F        ;Sets all bits in B except bit 7
    ///         
    /// See Also: BIOR, BOR, OIM, ORCC, ORD, ORR
    /// </remarks>
    public class DA_Orb_D : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);

            cpu.B_REG |= cpu.MemRead8(address);

            cpu.CC_N = NTEST8(cpu.B_REG);
            cpu.CC_Z = ZTEST(cpu.B_REG);
            cpu.CC_V = false;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 4);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._43);
    }
}
