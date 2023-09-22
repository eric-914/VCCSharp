using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>32/LEAS/INDEXED</code>
/// Load Effective Address into Register <c>S</c>
/// <code>S’ ← EA</code>
/// </summary>
/// <remarks>
/// These instructions compute the effective address from an Indexed Addressing Mode operand and place that address into the Index Registers <c>S</c>.
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// These instructions compute the effective address from an Indexed Addressing Mode operand and place that address into the Index Registers S.
/// 
/// The LEAS and LEAU instructions do not affect any of the Condition Code flags. 
/// 
/// LEA instructions differ from LD instructions in that the value loaded into the register is the address specified by the operand rather than the data pointed to by the address. 
/// LEA instructions might be used when you need to pass a parameter by-reference as opposed to by-value.
/// 
/// NOTE: The effective address of an auto-increment operand is the value prior to incrementing. 
/// Therefore, an instruction such as LEAS ,X+ will leave X unmodified. 
/// To achieve the expected results, you can use LEAS 1,X instead.
/// 
/// Cycles (4+)
/// Byte Count (2+)
/// 
/// See Also: ADDR, LD (16-bit), SUBR
internal class _32_Leas_X : OpCode, IOpCode
{
    public int CycleCount => 4;

    public void Exec()
    {
        S = INDEXED[PC++];
    }
}
