using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1.B
{
    /// <summary>
    /// EORA
    /// Exclusive or memory with accumulator
    /// EXTENDED
    /// Exclusively-OR Memory Byte with Accumulator A or B
    /// r’ ← r ⊕ (M)
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// EORA            EXTENDED            B8          5 / 4       3
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ 0  ]
    /// </summary>
    /// <remarks>
    /// These instructions exclusively-OR the contents of a byte in memory with either Accumulator A or B. 
    /// The 8-bit result is then placed in the specified accumulator.
    ///         N The Negative flag is set equal to the new value of bit 7 of the accumulator.
    ///         Z The Zero flag is set if the new value of the accumulator is zero; cleared otherwise.
    ///         V The Overflow flag is cleared by this instruction.
    ///         C The Carry flag is not affected by this instruction.
    ///         
    /// The EOR instruction produces a result containing '1' bits in the positions where the corresponding bits in the two operands have different values. 
    /// Exclusive-OR logic is often used in parity functions.
    /// 
    /// EOR can also be used to perform "bit-flipping" since a '1' bit in the source operand will invert the value of the corresponding bit in the destination operand. 
    /// For example:
    ///         EORA #%00000100 ;Invert value of bit 2 in Accumulator A
    ///         
    /// See Also: BEOR, BIEOR, EIM, EORD, EORR
    /// </remarks>
    public class B8_Eora_E : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.MemRead16(cpu.PC_REG);

            cpu.A_REG ^= cpu.MemRead8(address);

            cpu.CC_N = NTEST8(cpu.A_REG);
            cpu.CC_Z = ZTEST(cpu.A_REG);
            cpu.CC_V = false;

            cpu.PC_REG += 2;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 5);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._54);
    }
}
