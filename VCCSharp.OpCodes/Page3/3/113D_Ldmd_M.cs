using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>86/LDMD/IMMEDIATE</code>
/// Load an Immediate Value into the <c>MD</c> Register
/// <code>
/// MD.NM’ ← IMM.0
/// MD.FM’ ← IMM.1
/// </code>
/// </summary>
/// <remarks>
/// The <c>LDMD</c> instruction loads the two least-significant bits of the MD register (the Native Mode and FIRQ Mode control bits) with the two least-significant bits of the immediate operand.
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected.
/// 
/// The LDMD instruction provides the method by which the 6309 execution mode can be changed. 
/// Upon RESET, both the NM and FM mode bits are cleared. 
/// The execution mode may then be changed at any time by executing an LDMD instruction. 
/// 
/// Care should be taken when changing the value of the NM bit inside of an interrupt service routine because doing so can affect the behavior of an RTI instruction.
/// 
/// Bits 2 through 7 of the MD register are not affected by this instruction, so it cannot be used to alter the /0 and IL status bits.
/// 
/// The figure below shows the layout of the MD register:
///        7    6    5    4    3    2    1    0  
///     ╭────┬────┬────┬────┬────┬────┬────┬────╮
///     │ /0 │ IL │    │    │    │    │ FM │ NM |
///     ╰────┴────┴────┴────┴────┴────┴────┴────╯
/// 
/// LDMD #i8
/// I8 : 8-bit Immediate value
/// 
/// Cycles (5)
/// Byte Count (3)
/// 
/// See Also: BITMD, RTI
internal class _113D_Ldmd_M : OpCode6309, IOpCode
{
    public int Exec()
    {
        byte value = M8[PC++];

        MD = (byte)(value & 0x03);

        return 5;
    }
}
