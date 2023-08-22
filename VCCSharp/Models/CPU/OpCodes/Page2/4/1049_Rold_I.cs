using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// ROLD
    /// 🚫 6309 ONLY 🚫
    /// Rotate 16-Bit Accumulator Left through Carry
    /// INHERENT
    ///   ╭────────────────────────────────────────────────────────────────╮
    ///   │  ╭──╮     ╭──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──╮    │
    ///   ╰─ │  │ ◀── │  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │ ◀──╯
    ///      ╰──╯     ╰──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──╯
    ///       C       b15 ────────────────────────────────────────▶ b0       
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// ROLD            INHERENT            1049        3 / 2       2
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ ↕ ↕]
    /// </summary>
    /// <remarks>
    /// These instructions rotate the contents of the D or W accumulator to the left by one bit, through the Carry bit of the CC register (effectively a 17-bit rotation). 
    /// Bit 0 receives the original value of the Carry flag, while the Carry flag receives the original value of bit 15.
    ///         N The Negative flag is set equal to the new value of bit 15.
    ///         Z The Zero flag is set if the new 16-bit value is zero; cleared otherwise.
    ///         V The Overflow flag is set equal to the exclusive-OR of the original values of bits 14 and 15.
    ///         C The Carry flag receives the value shifted out of bit 15.
    /// 
    /// The ROL instructions can be used for subsequent words of a multi-byte shift to bring in the carry bit from a previous shift or rotate instruction. 
    /// Other uses include conversion of data from serial to parallel and vise-versa.
    /// 
    /// A left rotate of the 32-bit Q accumulator can be achieved by executing ROLW immediately followed by ROLD.
    /// 
    /// See Also: ROL (8-bit)
    /// </remarks>
    public class _1049_Rold_I : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            var bit = (ushort)(cpu.CC_C ? 1 : 0);

            cpu.CC_C = cpu.D_REG >> 15 != 0;
            cpu.CC_V = ((cpu.CC_C ? 1 : 0) ^ ((cpu.D_REG & 0x4000) >> 14)) != 0;
            cpu.D_REG = (ushort)((cpu.D_REG << 1) | bit);
            cpu.CC_Z = ZTEST(cpu.D_REG);
            cpu.CC_N = NTEST16(cpu.D_REG);

            return Cycles._32;
        }
    }
}
