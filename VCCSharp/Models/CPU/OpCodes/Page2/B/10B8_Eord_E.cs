using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2.B
{
    /// <summary>
    /// EORD
    /// 🚫 6309 ONLY 🚫
    /// Exclusively-OR Memory Word with Accumulator D
    /// EXTENDED
    /// ACCD’ ← ACCD ⊕ (M:M+1)
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// EORD            EXTENDED            10B8        8 / 6       4
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ 0  ]
    /// </summary>
    /// <remarks>
    /// The EORD instruction exclusively-ORs the contents of a double-byte value in memory with the contents of Accumulator D. 
    /// The 16-bit result is placed back into Accumulator D.
    ///         N The Negative flag is set equal to the new value of bit 15 of Accumulator D.
    ///         Z The Zero flag is set if the new value of the Accumulator D is zero; cleared otherwise.
    ///         V The Overflow flag is cleared by this instruction.
    ///         C The Carry flag is not affected by this instruction.
    ///         
    /// The EORD instruction produces a result containing '1' bits in the positions where the corresponding bits in the two operands have different values. 
    /// Exclusive-OR logic is often used in parity functions.
    /// 
    /// EOR can also be used to perform "bit-flipping" since a '1' bit in the source operand will invert the value of the corresponding bit in the destination operand. 
    /// For example:
    ///         EORD #$8080 ;Invert values of bits 7 and 15 in D
    ///         
    /// See Also: EOR (8-bit), EORR
    /// </remarks>
    public class _10B8_Eord_E : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.MemRead16(cpu.PC_REG);
            ushort value = cpu.MemRead16(address);

            cpu.D_REG ^= value;

            cpu.CC_N = NTEST16(cpu.D_REG);
            cpu.CC_Z = ZTEST(cpu.D_REG);
            cpu.CC_V = false;

            cpu.PC_REG += 2;

            return Cycles._86;
        }
    }
}
