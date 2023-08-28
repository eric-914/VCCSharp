using VCCSharp.OpCodes.Model.OpCodes;
using VCCSharp.OpCodes.Model.Support;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>1034/ANDR/IMMEDIATE</code>
/// Logically AND Source Register with Destination Register
/// <code>r1’ ← r1 AND r0</code>
/// </summary>
/// <remarks>
/// The <c>ANDR</c> instruction logically ANDs the contents of a source register with the contents of a destination register. 
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0  ]
/// 
/// The result is placed into the destination register.
///         N The Negative flag is set equal to the value of the result’s high-order bit.
///         Z The Zero flag is set if the new value of the destination register is zero; cleared otherwise.
///         V The Overflow flag is cleared by this instruction.
///         C The Carry flag is not affected by this instruction.
///         
/// Any of the 6309 registers except Q and MD may be specified as the source operand, destination operand or both; however specifying the PC register as either the source or destination produces undefined results.
/// 
/// The ANDR instruction will perform either an 8-bit or 16-bit operation according to the size of the destination register. 
/// When registers of different sizes are specified, the source will be promoted, demoted or substituted depending on the size of the destination and on which specific 8-bit register is involved. 
/// See “6309 Inter-Register Operations” on page 143 for further details.
/// 
/// The Immediate operand for this instruction is a postbyte which uses the same format as that used by the TFR and EXG instructions. 
/// For details, see the description of the TFR instruction.
/// 
/// Cycles (4)
/// Byte Count (3)
/// 
/// See Also: AND (8-bit), ANDCC, ANDD
internal class _1034_Andr : OpCode6309, IOpCode, IIndexedRegisterSwap
{
    private readonly IndexedRegisterSwap _irs;

    internal _1034_Andr(HD6309.IState cpu) : base(cpu)
    {
        _irs = new IndexedRegisterSwap(this, true)
        {
            F8 = (d,s) => Boolean((byte)(d & s)),
            F16 = (d,s) => Boolean((ushort)(d & s))
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
        //CC_C = f.C; //not applicable
    }
}
