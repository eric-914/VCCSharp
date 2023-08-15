using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3
{
    /// <summary>
    /// BITMD
    /// 🚫 6309 ONLY 🚫
    /// Bit Test the MD Register with an Immediate Value
    /// IMMEDIATE
    /// CC.Z ← (MD.IL AND IMM.6 = 0) AND (MD./0 AND IMM.7 = 0)
    ///                    ＿＿＿
    /// MD.IL’ ← MD.IL AND IMM.6
    ///                    ＿＿＿
    /// MD./0’ ← MD./0 AND IMM.7
    /// SOURCE FORM   ADDRESSING MODE   OPCODE      CYCLES      BYTE COUNT
    /// BITMD #i8     IMMEDIATE         113C        4           3
    /// 
    /// I8 : 8-bit Immediate value
    ///   [E F H I N Z V C]
    ///   [          ↕    ]
    /// </summary>
    /// <remarks>
    /// This instruction logically ANDs the two most-significant bits of the MD register (the Divide-by-Zero and Illegal Instruction status bits) with the two most-significant bits of the immediate operand. 
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
    /// Therefore BITMD cannot be used to determine or change the current execution mode of the CPU. 
    /// See “Determining the 6309 Execution Mode” on page 144 for more information on this topic.
    /// 
    /// The figure below shows the layout of the MD register:
    /// 
    ///        7    6   5   4   3   2   1   0  
    ///     ╭────┬────┬────┬────┬────┬────┬────┬────╮
    ///     │ /0 │ IL │    │    │    │    │ FM │ NM |
    ///     ╰────┴────┴────┴────┴────┴────┴────┴────╯
    /// 
    /// See Also: LDMD
    /// </remarks>
    public class _113C_Bitmd_M : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            byte value = cpu.MemRead8(cpu.PC_REG++);

            byte masked = (byte)(value & 0xC0);
            byte and = (byte)(cpu.MD & masked);

            cpu.CC_Z = ZTEST(and);

            if ((and & 0x80) != 0) cpu.MD_ZERODIV = false;
            if ((and & 0x40) != 0) cpu.MD_ILLEGAL = false;

            return 4;
        }
    }
}
