using VCCSharp.OpCodes.Model.OpCodes;
using VCCSharp.OpCodes.Model.Support;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>1035/ORR/IMMEDIATE</code>
/// Logically OR Source Register with Destination Register
/// <code>r1’ ← r1 OR r0</code>
/// </summary>
/// <remarks>
/// The <c>ORR</c> instruction logically ORs the contents of a source register with the contents of a destination register. 
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
/// All of the 6309 registers except Q and MD can be specified as either the source or destination; however specifying the PC register as either the source or destination produces undefined results.
/// 
/// Although the ORR instruction is capable of altering the flow of program execution by specifying the PC register as the destination, you should avoid doing so because the prefetch capability of the 6309 can produce un-predictable results.
/// 
/// See “6309 Inter-Register Operations” on page 143 for details on how this instruction operates when registers of different sizes are specified.
/// 
/// The Immediate operand for this instruction is a postbyte which uses the same format as that used by the TFR and EXG instructions. 
/// For details, see the description of the TFR instruction.
/// 
/// Cycles (4)
/// Byte Count (3)
/// 
/// See Also: OR (8-bit), ORD
internal class _1035_Orr_M : OpCode6309, IOpCode, IIndexedRegisterSwap
{
    private readonly IndexedRegisterSwap _irs;

    public int CycleCount => 4;

    internal _1035_Orr_M()
    {
        _irs = new IndexedRegisterSwap(this, true)
        {
            F8 = (d,s) => Boolean((byte)(d | s)),
            F16 = (d,s) => Boolean((ushort)(d | s))
        };
    }

    public void Exec()
    {
        byte value = M8[PC++];

        _irs.Exec(value);
    }

    public void SetFlags(IFlags f)
    {
        CC_N = f.N;
        CC_Z = f.Z;
        CC_V = f.V;
        //CC_C = f.C; //not applicable
    }
}
