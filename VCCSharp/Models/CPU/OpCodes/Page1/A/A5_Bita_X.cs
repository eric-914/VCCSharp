using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1.A
{
    /// <summary>
    /// BITA
    /// Bit test memory with accumulator
    /// Bit Test Accumulator A or B with Memory Byte Value
    /// INDEXED
    /// TEMP ← r AND (M)
    /// SOURCE FORM     ADDRESSING MODE     OPCODE       CYCLES     BYTE COUNT
    /// BITA            INDEXED             A5           4+         2+ 
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ 0  ]
    /// </summary>
    /// <remarks>
    /// These instructions logically AND the contents of a byte in memory with either Accumulator A or B. 
    /// The 8-bit result is tested to set or clear the appropriate flags in the CC register. 
    /// Neither the accumulator nor the memory byte are modified.
    ///     N The Negative flag is set equal to bit 7 of the resulting value.
    ///     Z The Zero flag is set if the resulting value was zero; cleared otherwise.
    ///     V The Overflow flag is cleared by this instruction.
    ///     C The Carry flag is not affected by this instruction.
    /// 
    /// The BIT instructions are used for testing bits. Consider the following example:
    ///     ANDA #%00000100 ;Sets Z flag if bit 2 is not set
    ///     
    /// BIT instructions differ from AND instructions only in that they do not modify the specified accumulator.
    /// 
    /// See Also: AND (8-bit), BITD, BITMD
    /// </remarks>
    public class A5_Bita_X : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.INDADDRESS(cpu.PC_REG++);
            byte value = (byte)(cpu.A_REG & cpu.MemRead8(address));

            cpu.CC_N = NTEST8(value);
            cpu.CC_Z = ZTEST(value);
            cpu.CC_V = false;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 4);
        public int Exec(IHD6309 cpu) => Exec(cpu, 4);
    }
}
