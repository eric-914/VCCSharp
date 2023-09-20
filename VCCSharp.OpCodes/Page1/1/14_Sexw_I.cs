using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>14/SEXW/INHERENT</code>
/// Sign Extend a 16-bit Value in W to a 32-bit Value in Q
/// <code>IF W.15=0 THEN D ← 0x0000</code>
/// <code>IF W.15=1 THEN D ← 0xFFFF</code>
/// </summary>
/// <remarks>
/// This instruction extends the 16-bit twos complement value in Accumulator W into a 32-bit twos complement value in Accumulator Q. 
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
///          ╭────────────────────────────────────────────Accumulator Q─────────────────────────────────────────╮ 
///      N   │                  Accumulator D                                      Accumulator W                │
///     ╭──╮ ╭──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──╮  ╭──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──╮ 
///     │  │ │  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │ b│15│  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │ 
///     ╰──╯ ╰──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──╯  ╰──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──╯ 
///      ▲    ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲     │
///      ╰────┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴─────╯
///      
/// [E F H I N Z V C]
/// [        ↕ ↕    ]
///   
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
/// Cycles (4)
/// Byte Count (1)
/// 
/// See Also: SEX
internal class _14_Sexw_I : OpCode6309, IOpCode
{
    public int CycleCount => 4;

    public int Exec()
    {
        D = (W & 32768) != 0 ? (ushort)0xFFFF : (ushort)0;

        CC_Z = Q == 0;
        CC_N = D.Bit15();

        return CycleCount;
    }
}