using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// RORW
    /// 🚫 6309 ONLY 🚫
    /// INHERENT
    ///     ╭───────────────────────────────────────────────────────────────╮
    ///     │   ╭──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──╮     ╭──╮  |
    ///     ╰─▶ │  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │  | ──▶ |  | -╯
    ///         ╰──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──╯     ╰──╯
    ///         b15 ──────────────────────────────────────-─▶ b0        C
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// RORW            INHERENT            1056        3 / 2       2
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕   ↕]
    /// </summary>
    /// <remarks>
    /// These instructions rotate the contents of the D or W accumulator to the right by one bit, through the Carry bit of the CC register (effectively a 17-bit rotation). 
    /// Bit 15 receives the original value of the Carry flag, while the Carry flag receives the original value of bit 0.
    ///         N The Negative flag is set equal to the new value of bit 15 (original value of Carry).
    ///         Z The Zero flag is set if the new 16-bit value is zero; cleared otherwise.
    ///         V The Overflow flag is not affected by these instructions.
    ///         C The Carry flag receives the value shifted out of bit 0.
    /// 
    /// The ROR instructions can be used for subsequent words of a multi-byte shift to bring in the carry bit from a previous shift or rotate instruction. 
    /// Other uses include conversion of data from serial to parallel and vise-versa.
    /// 
    /// A right rotate of the 32-bit Q accumulator can be achieved by executing RORD immediately followed by RORW.
    /// 
    /// See Also: ROR (8-bit)
    /// </remarks>
    public class _1056_Rorw_I : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            var bit = (ushort)((cpu.CC_C ? 1 : 0) << 15);

            cpu.CC_C = (cpu.W_REG & 1) != 0;
            cpu.W_REG = (ushort)((cpu.W_REG >> 1) | bit);
            cpu.CC_Z = ZTEST(cpu.W_REG);
            cpu.CC_N = NTEST16(cpu.W_REG);

            return Cycles._32;
        }
    }
}
