using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// BGT
    /// Branch if greater (signed) 
    /// Branch If Greater Than Zero
    /// RELATIVE
    /// IF (CC.N = CC.V) AND (CC.Z = 0) then PC’ ← PC + IMM
    /// SOURCE FORM   ADDRESSING MODE     OPCODE       CYCLES      BYTE COUNT
    /// BGT address   RELATIVE            2E           3           2
    ///   [E F H I N Z V C]
    ///   [               ]
    /// </summary>
    /// <remarks>
    /// This instruction tests the Zero (Z) flag in the CC register and, if it is clear AND the values of the Negative (N) and Overflow (V) flags are equal (both set OR both clear), causes a relative branch. 
    /// If the N and V flags do not have the same value or if the Z flag is set then the CPU continues executing the next instruction in sequence. 
    /// None of the Condition Code flags are affected by this instruction.
    /// 
    /// When used following a subtract or compare of signed (twos-complement) values, the BGT instruction will branch if the source value was greater than the original destination value.
    /// 
    /// The branch address is calculated by adding the current value of the PC register (after the BGT instruction bytes have been fetched) with the 8-bit twos-complement value contained in the second byte of the instruction. 
    /// The range of the branch destination is limited to -126 to +129 bytes from the address of the BGT instruction. 
    /// If a larger range is required then the LBGT instruction may be used instead.
    /// 
    /// See Also: BHI, BLE, LBGT
    /// </remarks>
    public class _2E_Bgt_R : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            if (!(cpu.CC_Z | (cpu.CC_N ^ cpu.CC_V)))
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
