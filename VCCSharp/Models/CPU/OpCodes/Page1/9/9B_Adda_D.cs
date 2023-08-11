using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    public class _9B_Adda_D : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);
            byte _postByte = cpu.MemRead8(address);
            ushort _temp16 = (ushort)(cpu.A_REG + _postByte);

            cpu.CC_C = (_temp16 & 0x100) >> 8 != 0;
            cpu.CC_H = ((cpu.A_REG ^ _postByte ^ _temp16) & 0x10) >> 4 != 0;
            cpu.CC_V = OVERFLOW8(cpu.CC_C, _postByte, _temp16, cpu.A_REG);

            cpu.A_REG = (byte)_temp16;

            cpu.CC_N = NTEST8(cpu.A_REG);
            cpu.CC_Z = ZTEST(cpu.A_REG);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 4);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._43);
    }
}
