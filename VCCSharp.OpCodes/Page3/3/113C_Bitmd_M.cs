using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>113C/BITMD/IMMEDIATE</code>
/// Bit Test the <c>MD</c> Register with an Immediate Value
/// <code>
///   CC.Z ← (MD.IL AND IMM.6 = 0) AND (MD./0 AND IMM.7 = 0)
///                    ＿＿＿
/// MD.IL’ ← MD.IL AND IMM.6
///                    ＿＿＿
/// MD./0’ ← MD./0 AND IMM.7
/// </code>
/// </summary>
/// <remarks>
/// The <c>BITMD</c> instruction logically ANDs the two most-significant bits of the MD register (the Divide-by-Zero and Illegal Instruction status bits) with the two most-significant bits of the immediate operand. 
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [          ↕    ]
/// 
/// The Z flag in the CC register is set if the AND operation produces a zero result, otherwise Z is cleared. 
/// No other condition code flags are affected.
/// The BITMD instruction also clears those status bits in the MD register which correspond to '1' bits in the immediate operand. 
/// The values of bits 0 through 5 in the immediate operand have no relevance and do not affect the operation of the BITMD instruction in any way.
/// 
/// The BITMD instruction provides a method to test the Divide-by-Zero (/0) and Illegal Instruction (IL) status bits of the MD register after an Illegal Instruction Exception has occurred. 
/// At most, only one of these flags will be set, indicating which condition caused the exception. 
/// Since the status bit(s) tested are also cleared by this instruction, you can only test for each condition once.
/// 
/// Bits 0 through 5 of the MD register are neither tested nor cleared by this instruction.
/// Therefore BITMD cannot be used to determine or change the current execution mode of the  
/// See “Determining the 6309 Execution Mode” on page 144 for more information on this topic.
/// 
/// The figure below shows the layout of the MD register:
///        7    6    5    4    3    2    1    0  
///     ╭────┬────┬────┬────┬────┬────┬────┬────╮
///     │ /0 │ IL │    │    │    │    │ FM │ NM |
///     ╰────┴────┴────┴────┴────┴────┴────┴────╯
/// 
/// BITMD #i8
/// I8 : 8-bit Immediate value
/// 
/// Cycles (4)
/// Byte Count (3)
/// 
/// See Also: LDMD
internal class _113C_Bitmd_M : OpCode6309, IOpCode
{
    internal _113C_Bitmd_M(HD6309.IState cpu) : base(cpu) { }

    public int Exec()
    {
        byte value = M8[PC++];

        byte result = (byte)(MD & value & 0xC0);

        CC_Z = result == 0;

        MD_ZERODIV = !result.Bit7();
        MD_ILLEGAL = !result.Bit6();

        return 4;
    }
}
