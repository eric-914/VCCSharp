﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1.D
{
    //ADCB
    //Add memory to accumulator with carry
    //DIRECT
    public class D9_Adcb_D : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);
            byte value = cpu.MemRead8(address);
            ushort sum = (ushort)(cpu.B_REG + value + (cpu.CC_C ? 1 : 0));

            cpu.CC_C = (sum & 0x100) >> 8 != 0;
            cpu.CC_V = OVERFLOW8(cpu.CC_C, value, sum, cpu.B_REG);
            cpu.CC_H = ((cpu.B_REG ^ sum ^ value) & 0x10) >> 4 != 0;

            cpu.B_REG = (byte)sum;

            cpu.CC_N = NTEST8(cpu.B_REG);
            cpu.CC_Z = ZTEST(cpu.B_REG);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 4);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._43);
    }
}