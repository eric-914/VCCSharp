using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// SEXW
    /// 🚫 6309 ONLY 🚫
    /// Sign Extend a 16-bit Value in W to a 32-bit Value in Q
    /// INHERENT
    ///          ╭────────────────────────────────────────────Accumulator Q─────────────────────────────────────────╮ 
    ///      N   │                  Accumulator D                                      Accumulator W                │
    ///     ╭──╮ ╭──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──╮  ╭──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──╮ 
    ///     │  │ │  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │ b│15│  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │ 
    ///     ╰──╯ ╰──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──╯  ╰──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──╯ 
    ///      ▲    ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲     │
    ///      ╰────┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴─────╯
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// SEXW            INHERENT            14          4           1
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕    ]
    /// </summary>
    /// <remarks>
    /// This instruction extends the 16-bit twos complement value in Accumulator W into a 32-bit twos complement value in Accumulator Q. 
    /// This is accomplished by copying the value of bit 15 (the sign bit) from Accumulator W into all 16 bits of Accumulator D.
    ///         N The Negative flag is also set equal the value of bit 15 in Accumulator W
    ///         Z The Zero flag is set if the new value of Accumulator Q is zero (W was zero); cleared otherwise.
    ///         V The Overflow flag is not affected by this instruction.
    ///         C The Carry flag is not affected by this instruction.
    ///         
    /// The SEXW instruction is used when a signed (twos complement) 16-bit value needs to be promoted to a full 32-bit value. 
    /// For unsigned arithmetic, promoting a 16-bit value in Accumulator W to a 32-bit value in Accumulator Q requires zero-extending the value by executing a CLRD instruction instead.
    /// 
    /// You can sign extend an 8-bit value in Accumulator A to a 32-bit value in Accumulator Q by executing the following sequence of instructions:
    ///         SEX     ; Sign extend A into D
    ///         TFR D,W ; Move D to W
    ///         SEXW    ; Sign extend W into Q
    ///         
    /// See Also: SEX
    /// </remarks>
    public class _14_Sexw_I : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu)
        {
            throw new NotImplementedException();
        }

        public int Exec(IHD6309 cpu)
        {
            cpu.D_REG = (cpu.W_REG & 32768) != 0 ? (ushort)0xFFFF : (ushort)0;

            cpu.CC_Z = ZTEST(cpu.Q_REG);
            cpu.CC_N = NTEST16(cpu.D_REG);

            return 4;
        }
    }
}
