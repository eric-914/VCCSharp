using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1.A
{
    /// <summary>
    /// EORA
    /// Exclusive or memory with accumulator
    /// INDEXED
    /// Exclusively-OR Memory Byte with Accumulator A or B
    /// r’ ← r ⊕ (M)
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// EORA            INDEXED             A8          4+          2+ 
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
    public class A8_Eora_X : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.INDADDRESS(cpu.PC_REG++);

            cpu.A_REG ^= cpu.MemRead8(address);

            cpu.CC_N = NTEST8(cpu.A_REG);
            cpu.CC_Z = ZTEST(cpu.A_REG);
            cpu.CC_V = false;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 4);
        public int Exec(IHD6309 cpu) => Exec(cpu, 4);
    }
}
