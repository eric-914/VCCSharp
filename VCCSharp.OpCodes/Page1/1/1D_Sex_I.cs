using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>1D/SEX/INHERENT</code>
/// Sign Extend the 8-bit Value in B to a 16-bit Value in D
/// <code>IF B.7=0 THEN A ← 0x00</code>
/// <code>IF B.7=1 THEN A ← 0xFF</code>
/// </summary>
/// <remarks>
/// This instruction extends the 8-bit twos complement value in Accumulator B into a 16-bit twos complement value in Accumulator D. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕    ]
///   
///          ╭──────────────────Accumulator D──────────────────╮ 
///      N   │     Accumulator A             Accumulator B     │
///     ╭──╮ ╭──┬──┬──┬──┬──┬──┬──┬──╮ ╭──┬──┬──┬──┬──┬──┬──┬──╮ 
///     │  │ │  │  │  │  │  │  │  │  │ │b7│  │  │  │  │  │  │  │ 
///     ╰──╯ ╰──┴──┴──┴──┴──┴──┴──┴──╯ ╰──┴──┴──┴──┴──┴──┴──┴──╯ 
///      ▲    ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲    │
///      ╰────┴──┴──┴──┴──┴──┴──┴──┴────╯
///      
/// This instruction extends the 8-bit twos complement value in Accumulator B into a 16-bit twos complement value in Accumulator D. 
/// This is accomplished by copying the value of bit 7 (the sign bit) from Accumulator B into all 8 bits of Accumulator A.
///     N The Negative flag is also set equal the value of bit 7 in Accumulator B
///     Z The Zero flag is set if the new value of Accumulator D is zero (B was zero); cleared otherwise.
/// 
/// The SEX instruction is used when a signed (twos complement) 8-bit value needs to be promoted to a full 16-bit value. 
/// For unsigned arithmetic, promoting an 8-bit value in Accumulator A to a 16-bit value in Accumulator D requires zero-extending the value by executing a CLRA instruction instead.
/// 
/// On a 6309, you can sign extend an 8-bit value in Accumulator A to a 32-bit value in Accumulator Q by executing the following sequence of instructions:
///         SEX     ; Sign extend A into D
///         TFR D,W ; Move D to W
///         SEXW    ; Sign extend W into Q
/// 
/// Cycles (2 / 1)
/// Byte Count (1)
/// 
/// See Also: SEXW
internal class _1D_Sex_I : OpCode, IOpCode
{
    public int CycleCount => DynamicCycles._21;

    public void Exec()
    {
        bool bit = B.Bit7();

        A = bit ? (byte)0xFF : (byte)0x00;

        CC_Z = D == 0;
        CC_N = bit;
    }
}
