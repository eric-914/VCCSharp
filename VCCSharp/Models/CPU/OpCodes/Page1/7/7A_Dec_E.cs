using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// DEC
    /// Decrement accumulator or memory location
    /// Decrement a Byte in Memory
    /// EXTENDED
    /// (M)’ ← (M) - 1
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// DEC             EXTENDED            7A          7 / 6       3
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ ↕  ]
    /// </summary>
    /// <remarks>
    /// This instruction subtracts 1 from the value contained in a memory byte. 
    /// The Condition Code flags are also modified as follows:
    ///         N The Negative flag is set equal to the new value of bit 7.
    ///         Z The Zero flag is set if the new value of the memory byte is zero; cleared otherwise.
    ///         V The Overflow flag is set if the original value of the memory byte was $80; cleared otherwise.
    ///         C The Carry flag is not affected by this instruction.
    ///         
    /// Because the DEC instruction does not affect the Carry flag, it can be used to implement a loop counter within a multiple precision computation.
    /// When used to decrement an unsigned value, only the BEQ and BNE branches will always behave as expected. 
    /// When operating on a signed value, all of the signed conditional branch instructions will behave as expected.
    /// 
    /// See Also: DEC (accumulator), INC, SUB
    /// </remarks>
    public class _7A_Dec_E : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.MemRead16(cpu.PC_REG);
            byte value = cpu.MemRead8(address);

            value--;

            cpu.CC_Z = ZTEST(value);
            cpu.CC_N = NTEST8(value);
            cpu.CC_V = value == 0x7F;

            cpu.MemWrite8(value, address);

            cpu.PC_REG += 2;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 7);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._76);
    }
}
