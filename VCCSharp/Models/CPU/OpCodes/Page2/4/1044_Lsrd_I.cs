using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// LSRD
    /// 🚫 6309 ONLY 🚫
    /// Logical Shift Right of 16-Bit Accumulator
    /// INHERENT
    ///          ╭──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──╮     ╭──╮
    ///    0 ──▶ │  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │  | ──▶ |  |
    ///          ╰──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──╯     ╰──╯
    ///           b15 ───────────────────────────────────────▶ b0       C
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// LSRD            INHERENT            1044        3 / 2       2
    ///   [E F H I N Z V C]
    ///   [        0 ↕   ↕]
    /// </summary>
    /// <remarks>
    /// This instruction shifts the contents of Accumulator D to the right by one bit. 
    /// Bit 0 is shifted into the Carry flag of the Condition Codes register. 
    /// The value of bit 15 is not changed.
    ///         N The Negative flag is cleared by these instructions.
    ///         Z The Zero flag is set if the new 16-bit value is zero; cleared otherwise.
    ///         V The Overflow flag is not affected by this instruction.
    ///         C The Carry flag receives the value shifted out of bit 0.
    ///         
    /// These instructions can be used in simple division routines on unsigned values (a single right-shift divides the value by 2).
    /// 
    /// A logical right-shift of the 32-bit Q accumulator can be achieved as follows:
    ///         LSRD ; Shift Hi-word, Low-bit into Carry
    ///         RORW ; Shift Low-word, Carry into Hi-bit
    /// 
    /// See Also: LSR (8-bit), ROR (16-bit)
    /// </remarks>
    public class _1044_Lsrd_I : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            cpu.CC_C = (cpu.D_REG & 1) != 0;
            cpu.D_REG = (ushort)(cpu.D_REG >> 1);
            cpu.CC_Z = ZTEST(cpu.D_REG);
            cpu.CC_N = false;

            return Cycles._32;
        }
    }
}
