using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// BLE
    /// Branch if less than or equal (signed)
    /// Branch If Less than or Equal to Zero
    /// RELATIVE
    /// IF (CC.N ≠ CC.V) OR (CC.Z = 1) then PC’ ← PC + IMM
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES     BYTE COUNT
    /// BLE             RELATIVE            2F          3          2
    ///   [E F H I N Z V C]
    ///   [               ]
    /// </summary>
    /// <remarks>
    /// This instruction performs a relative branch if the value of the Zero (Z) flag is 1, OR if the values of the Negative (N) and Overflow (V) flags are not equal. 
    /// If the N and V flags have the same value and the Z flag is not set then the CPU continues executing the next instruction in sequence. 
    /// None of the Condition Code flags are affected by this instruction.
    /// 
    /// When used following a subtract or compare of signed (twos-complement) values, the BLE instruction will branch if the source value was less than or equal to the original destination value.
    /// 
    /// The branch address is calculated by adding the current value of the PC register (after the BLE instruction bytes have been fetched) with the 8-bit twos-complement value contained in the second byte of the instruction. 
    /// The range of the branch destination is limited to -126 to +129 bytes from the address of the BLE instruction. If a larger range is required then the LBLE instruction may be used instead.
    /// 
    /// See Also: BGT, BLS, LBLE
    /// </remarks>
    public class _2F_Ble_R : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            if (cpu.CC_Z | (cpu.CC_N ^ cpu.CC_V))
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
