using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// BLT
    /// Branch if less than (signed)
    /// Branch If Less Than Zero
    /// RELATIVE
    /// IF CC.N ≠ CC.V then PC’ ← PC + IMM
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// BLT address     RELATIVE            2D          3           2
    ///   [E F H I N Z V C]
    ///   [               ]
    /// </summary>
    /// <remarks>
    /// This instruction performs a relative branch if the values of the Negative (N) and Overflow (V) flags are not equal. 
    /// If the N and V flags have the same value then the CPU continues executing the next instruction in sequence. 
    /// None of the Condition Code flags are affected by this instruction.
    /// When used following a subtract or compare of signed (twos-complement) values, the BLT instruction will branch if the source value was less than the original destination value.
    /// The branch address is calculated by adding the current value of the PC register (after the BLT instruction bytes have been fetched) with the 8-bit twos-complement value contained in the second byte of the instruction. 
    /// The range of the branch destination is limited to -126 to +129 bytes from the address of the BLT instruction. 
    /// If a larger range is required then the LBLT instruction may be used instead.
    /// 
    /// See Also: BGE, BLO, LBLT
    /// </remarks>
    public class _2D_Blt_R : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            if (cpu.CC_V ^ cpu.CC_N)
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
