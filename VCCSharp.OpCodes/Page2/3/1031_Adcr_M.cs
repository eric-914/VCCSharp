using VCCSharp.OpCodes.Model.OpCodes;
using VCCSharp.OpCodes.Model.Support;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>1031/ADCR/IMMEDIATE</code>
/// Add Source Register plus Carry to Destination Register
/// <code>r1’ ← r1 + r0 + C</code>
/// </summary>
/// <remarks>
/// The <c>ADCR</c> instruction adds the contents of a source register plus the contents of the Carry flag with the contents of a destination register. 
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ ↕ ↕]
/// 
/// The result is placed into the destination register.
///         N The Negative flag is set equal to the value of the result’s high-order bit.
///         Z The Zero flag is set if the new value of the destination register is zero; cleared otherwise.
///         V The Overflow flag is set if an overflow occurred; cleared otherwise.
///         C The Carry flag is set if a carry out of the high-order bit occurred; cleared otherwise.
///     
/// Any of the 6309 registers except Q and MD may be specified as the source operand, destination operand or both; however specifying the PC register as either the source or destination produces undefined results.
/// The ADCR instruction will perform either 8-bit or 16-bit addition according to the size of the destination register. 
/// When registers of different sizes are specified, the source will be promoted, demoted or substituted depending on the size of the destination and on which specific 8-bit register is involved. 
/// See “6309 Inter-Register Operations” on page 143 for further details.
/// The Immediate operand for this instruction is a postbyte which uses the same format as that used by the TFR and EXG instructions. 
/// See the description of the TFR instruction for further details.
/// 
/// Cycles (4)
/// Byte Count (3)
/// 
/// See Also: ADC (8-bit), ADCD
internal class _1031_Adcr_M : OpCode6309, IOpCode, IIndexedRegisterSwap
{
    private readonly IndexedRegisterSwap _irs;

    public int CycleCount => 4;

    internal _1031_Adcr_M()
    {
        _irs = new IndexedRegisterSwap(this, true)
        {
            F8 = (d,s) => Add(d, s, CC_C),
            F16 = (d,s) => Add(d, s, CC_C)
        };
    }

    public int Exec()
    {
        byte value = M8[PC++];

        _irs.Exec(value);

        return CycleCount;
    }

    public void SetFlags(IFlags f)
    {
        CC_N = f.N;
        CC_Z = f.Z;
        CC_V = f.V;
        CC_C = f.C;
    }
}
