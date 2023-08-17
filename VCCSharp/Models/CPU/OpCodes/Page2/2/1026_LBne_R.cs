using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// LBNE
    /// Branch if not equal
    /// Long Branch If Not Equal to Zero
    /// RELATIVE
    /// IF CC.Z = 0 then PC’ ← PC + IMM
    /// SOURCE FORM         ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// LBNE address        RELATIVE            1026        5 (6) *     4
    /// *The 6809 requires 6 cycles only if the branch is taken
    ///   [E F H I N Z V C]
    ///   [               ]
    /// </summary>
    /// <remarks>
    /// This instruction tests the Zero flag in the CC register and, if it is clear (0), causes a relative branch. 
    /// If the Z flag is set, the CPU continues executing the next instruction in sequence. 
    /// None of the Condition Code flags are affected by this instruction.
    /// 
    /// When used following almost any instruction that produces, tests or moves a value, the LBNE instruction will branch if that value is not equal to zero. 
    /// In the case of an instruction that performs a subtract or compare, the LBNE instruction will branch if the source value was not equal to the original destination value.
    /// 
    /// LBNE is generally not useful following a CLR instruction since the Z flag is always set.
    /// 
    /// The following instructions produce or move values, but do not affect the Z flag:
    ///         ABX   BAND  BEOR  BIAND  BIEOR
    ///         BOR   BIOR  EXG   LDBT   LDMD
    ///         LEAS  LEAU  PSH   PUL    STBT
    ///         TFM   TFR
    ///         
    /// The branch address is calculated by adding the current value of the PC register (after the LBNE instruction bytes have been fetched) with the 16-bit twos-complement value contained in the third and fourth bytes of the instruction. 
    /// Long branch instructions permit a relative jump to any location within the 64K address space. 
    /// The smaller, faster BNE instruction can be used instead when the destination address is within -126 to +129 bytes of the address of the branch instruction.
    /// 
    /// See Also: BNE, LBEQ
    /// </remarks>
    public class _1026_LBne_R : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            if (!cpu.CC_Z)
            {
                cpu.PC_REG += (ushort)(short)cpu.MemRead16(cpu.PC_REG);

                cycles += 1;
            }

            cpu.PC_REG += 2;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 5);
        public int Exec(IHD6309 cpu) => Exec(cpu, 5);
    }
}
