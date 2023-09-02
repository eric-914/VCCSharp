using VCCSharp.OpCodes.Model.OpCodes;
using VCCSharp.OpCodes.Model.Support;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>1037/CMPR/IMMEDIATE</code>
/// Compare Source Register from Destination Register
/// <code>TEMP ← r1 - r0</code>
/// </summary>
/// <remarks>
/// The <c>CMPR</c> instruction subtracts the contents of a source register from the contents of a destination register and sets the Condition Codes accordingly. 
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ ↕ ↕]
/// 
/// Neither register is modified.
///         N The Negative flag is set equal to the value of the high-order bit of the result.
///         Z The Zero flag is set if the resulting value is zero; cleared otherwise.
///         V The Overflow flag is set if an overflow occurred; cleared otherwise.
///         C The Carry flag is set if a borrow into the high-order bit was needed; cleared otherwise.
/// 
/// Any of the 6309 registers except Q and MD may be specified as the source operand, destination operand or both; however specifying the PC register as either the source or destination produces undefined results.
/// 
/// The CMPR instruction will perform either an 8-bit or 16-bit comparison according to the size of the destination register. 
/// When registers of different sizes are specified, the source will be promoted, demoted or substituted depending on the size of the destination and on which specific 8-bit register is involved. 
/// See “6309 Inter-Register Operations” on page 143 for further details.
/// 
/// The Immediate operand for this instruction is a postbyte which uses the same format as that used by the TFR and EXG instructions. 
/// See the description of the TFR instruction starting on page 137 for further details.
/// 
/// Cycles (4)
/// Byte Count (3)
/// 
/// See Also: ADD (8-bit), ADD (16-bit)
internal class _1037_Cmpr_M : OpCode6309, IOpCode, IIndexedRegisterSwap
{
    private readonly IndexedRegisterSwap _irs;

    internal _1037_Cmpr_M()
    {
        _irs = new IndexedRegisterSwap(this, false)
        {
            F8 = Subtract,
            F16 = Subtract,
        };
    }

    public int Exec()
    {
        byte value = M8[PC++];

        _irs.Exec(value);

        return 4;
    }

    public void SetFlags(IFlags f)
    {
        CC_N = f.N;
        CC_Z = f.Z;
        CC_V = f.V;
        CC_C = f.C;
    }
}
