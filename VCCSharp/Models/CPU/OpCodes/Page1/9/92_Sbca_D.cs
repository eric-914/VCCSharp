using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// SBCA
    /// Subtract memory from accumulator with borrow
    /// Subtract Memory Byte and Carry from Accumulator A or B
    /// DIRECT
    /// r’ ← r - IMM8|(M) - C
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// SBCA            DIRECT              92          4 / 3       2 
    ///   [E F H I N Z V C]
    ///   [    ~   ↕ ↕ ↕ ↕]
    /// </summary>
    /// <remarks>
    /// These instructions subtract either an 8-bit immediate value or the contents of a memory byte, plus the value of the Carry flag from the A or B accumulator. 
    /// The 8-bit result is placed back into the specified accumulator. 
    /// Note that since subtraction is performed, the purpose of the Carry flag is to represent a Borrow.
    ///         H The affect on the Half-Carry flag is undefined for these instructions.
    ///         N The Negative flag is set equal to the new value of bit 7 of the accumulator.
    ///         Z The Zero flag is set if the new accumulator value is zero; cleared otherwise.
    ///         V The Overflow flag is set if an overflow occurred; cleared otherwise.
    ///         C The Carry flag is set if a borrow into bit-7 was needed; cleared otherwise.
    /// 
    /// The SBC instruction is most often used to perform subtraction of the subsequent bytes of a multi-byte subtraction. 
    /// This allows the borrow from a previous SUB or SBC instruction to be included when doing subtraction for the next higher-order byte.
    /// Since the 6809 and 6309 both provide 16-bit SUB instructions for the accumulators, it is not necessary to use the 8-bit SUB and SBC instructions to perform 16-bit subtraction.
    /// 
    /// See Also: SBCD, SBCR
    /// </remarks>
    public class _92_Sbca_D : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);
            byte value = cpu.MemRead8(address);
            ushort compare = (ushort)(cpu.A_REG - value - (cpu.CC_C ? (byte)1 : (byte)0));

            cpu.CC_C = (compare & 0x100) >> 8 != 0;
            cpu.CC_V = OVERFLOW8(cpu.CC_C, value, compare, cpu.A_REG);

            cpu.A_REG = (byte)compare;

            cpu.CC_N = NTEST8(cpu.A_REG);
            cpu.CC_Z = ZTEST(cpu.A_REG);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 4);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._43);
    }
}
