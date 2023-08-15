using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// BMI
    /// Branch if minus 
    /// RELATIVE
    /// IF CC.N ≠ 0 then PC’ ← PC + IMM
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// BMI address     RELATIVE            2B          3           2
    ///   [E F H I N Z V C]
    ///   [               ]
    /// </summary>
    /// <remarks>
    /// This instruction tests the Negative (N) flag in the CC register and, if it is set (1), causes a relative branch. 
    /// If the N flag is 0, the CPU continues executing the next instruction in sequence. 
    /// None of the Condition Code flags are affected by this instruction.
    /// 
    /// When used following an operation on signed (twos-complement) binary values, the BMI instruction will branch if the resulting value is negative. 
    /// It is generally preferable to use the BLT instruction following such an operation because the sign bit may be invalid due to a twos-complement overflow.
    /// 
    /// The branch address is calculated by adding the current value of the PC register (after the BMI instruction bytes have been fetched) with the 8-bit twos-complement value contained in the second byte of the instruction. 
    /// The range of the branch destination is limited to -126 to +129 bytes from the address of the BMI instruction. 
    /// If a larger range is required then the LBMI instruction may be used instead.
    /// 
    /// See Also: BLT, BPL, LBMI
    /// </remarks>
    public class _2B_Bmi_R : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            if (cpu.CC_N)
            {
                cpu.PC_REG += (ushort)(sbyte)cpu.MemRead8(cpu.PC_REG);
            }

            cpu.PC_REG++;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 3);
        public int Exec(IHD6309 cpu) => Exec(cpu, 3);
    }
}
