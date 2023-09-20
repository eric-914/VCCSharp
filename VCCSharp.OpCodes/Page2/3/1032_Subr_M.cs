using VCCSharp.OpCodes.Model.OpCodes;
using VCCSharp.OpCodes.Model.Support;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>1032/SUBR/IMMEDIATE</code>
/// Subtract Source Register from Destination Register
/// <code>r1’ ← r1 - r0</code>
/// </summary>
/// <remarks>
/// The <c>SUBR</c> instruction subtracts the value contained in the source register from the value contained in the destination register. 
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
///         C The Carry flag is set if a borrow into the high-order bit was needed; cleared otherwise.
///         
/// Note that since subtraction is performed, the purpose of the Carry flag is to represent a Borrow.
///         
/// All of the 6309 registers except Q and MD can be specified as either the source or destination; however specifying the PC register as either the source or destination produces undefined results.
/// 
/// The SUBR instruction will perform either 8-bit or 16-bit subtraction according to the size of the destination register. 
/// When registers of different sizes are specified, the source will be promoted, demoted or substituted depending on the size of the destination and on which specific 8-bit register is involved. 
/// See “6309 Inter-Register Operations” on page 143 for further details.
/// 
/// Although the SUBR instruction is capable of altering the flow of program execution by specifying the PC register as the destination, you should avoid doing so because the prefetch capability of the 6309 can produce un-predictable results.
/// 
/// The Immediate operand for this instruction is a postbyte which uses the same format as that used by the TFR and EXG instructions. 
/// See the description of the TFR instruction for further details.
/// 
/// Cycles (4)
/// Byte Count (3)
/// 
/// See Also: SUB (8-bit), SUB (16-bit)
internal class _1032_Subr_M : OpCode6309, IOpCode, IIndexedRegisterSwap
{
    private readonly IndexedRegisterSwap _irs;

    public int CycleCount => 4;

    internal _1032_Subr_M()
    {
        _irs = new IndexedRegisterSwap(this, true)
        {
            F8 = Subtract,
            F16 = Subtract
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
