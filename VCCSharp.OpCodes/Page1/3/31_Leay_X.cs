using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>31/LEAY/INDEXED</code>
/// Load Effective Address into Register <c>Y</c>
/// <code>Y’ ← EA</code>
/// </summary>
/// <remarks>
/// These instructions compute the effective address from an Indexed Addressing Mode operand and place that address into the Index Registers <c>Y</c>.
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [          ↕    ]
/// 
/// These instructions compute the effective address from an Indexed Addressing Mode operand and place that address into the Index Registers Y.
/// 
/// The LEAX and LEAY instructions set the Z flag when the effective address is 0 and clear it otherwise. 
/// This permits X and Y to be used as 16-bit loop counters as well as providing compatibility with the INX and DEX instructions of the 6800 microprocessor.
/// 
/// LEA instructions differ from LD instructions in that the value loaded into the register is the address specified by the operand rather than the data pointed to by the address. 
/// LEA instructions might be used when you need to pass a parameter by-reference as opposed to by-value.
/// 
/// NOTE: The effective address of an auto-increment operand is the value prior to incrementing. 
/// Therefore, an instruction such as LEAY ,X+ will leave X unmodified. 
/// To achieve the expected results, you can use LEAY 1,X instead.
/// 
/// Cycles (4+)
/// Byte Count (2+)
/// 
/// See Also: ADDR, LD (16-bit), SUBR
internal class _31_Leay_X : OpCode, IOpCode
{
    internal _31_Leay_X(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        Y = INDEXED[PC++];

        CC_Z = Y == 0;

        return 4;
    }
}
