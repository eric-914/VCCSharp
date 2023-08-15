using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// BVS
    /// Branch if overflow set
    /// Branch if invalid 2's complement result 
    /// RELATIVE
    /// IF CC.V ≠ 0 then PC’ ← PC + IMM
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES     BYTE COUNT
    /// BVS address     RELATIVE            29          3          2
    ///   [E F H I N Z V C]
    ///   [               ]
    /// </summary>
    /// <remarks>
    /// This instruction tests the Overflow (V) flag in the CC register and, if it is set (1), causes a relative branch. 
    /// If the V flag is clear, the CPU continues executing the next instruction in sequence. 
    /// None of the Condition Code flags are affected by this instruction.
    /// 
    /// When used following an operation on signed (twos-complement) binary values, the BVS instruction will branch if an overflow occurred.
    /// 
    /// The branch address is calculated by adding the current value of the PC register (after the BVS instruction bytes have been fetched) with the 8-bit twos-complement value contained in the second byte of the instruction. 
    /// The range of the branch destination is limited to -126 to +129 bytes from the address of the BVS instruction. 
    /// If a larger range is required then the LBVS instruction may be used instead.
    /// 
    /// See Also: BVC, LBVS
    /// </remarks>
    public class _29_Bvs_R : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            if (cpu.CC_V)
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
