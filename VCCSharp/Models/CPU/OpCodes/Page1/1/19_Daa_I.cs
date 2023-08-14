using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //DAA
    //Decimal adjust A accumulator
    //INHERENT
    public class _19_Daa_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            byte msn, lsn;

            msn = (byte)(cpu.A_REG & 0xF0);
            lsn = (byte)(cpu.A_REG & 0xF);

            byte mask = 0;

            if (cpu.CC_H || lsn > 9)
            {
                mask |= 0x06;
            }

            if (msn > 0x80 && lsn > 9)
            {
                mask |= 0x60;
            }

            if (msn > 0x90 || cpu.CC_C)
            {
                mask |= 0x60;
            }

            ushort value = (ushort)(cpu.A_REG + mask);

            cpu.CC_C |= (value & 0x100) >> 8 != 0;
            cpu.A_REG = (byte)value;
            cpu.CC_N = NTEST8(cpu.A_REG);
            cpu.CC_Z = ZTEST(cpu.A_REG);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 2);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._21);
    }
}
