using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3
{
    /// <summary>
    /// LDMD
    /// 🚫 6309 ONLY 🚫
    /// Load an Immediate Value into the MD Register
    /// IMMEDIATE
    /// MD.NM’ ← IMM.0
    /// MD.FM’ ← IMM.1
    /// SOURCE FORM         ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// LDMD #i8            IMMEDIATE           113D        5           3
    /// 
    /// I8 : 8-bit Immediate value
    ///   [E F H I N Z V C]
    ///   [               ]
    /// </summary>
    /// <remarks>
    /// This instruction loads the two least-significant bits of the MD register (the Native Mode and FIRQ Mode control bits) with the two least-significant bits of the immediate operand. 
    /// None of the Condition Code flags are affected.
    /// 
    /// The LDMD instruction provides the method by which the 6309 execution mode can be changed. 
    /// Upon RESET, both the NM and FM mode bits are cleared. 
    /// The execution mode may then be changed at any time by executing an LDMD instruction. 
    /// See page 144 for more information about the 6309 execution modes.
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
    /// See Also: BITMD, RTI
    /// </remarks>
    public class _113D_Ldmd_M : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            byte value = cpu.MemRead8(cpu.PC_REG++);

            cpu.MD = (byte)(value & 0x03);

            return 5;
        }
    }
}
