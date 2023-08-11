﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    public class _93_Subd_D : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);
            ushort _temp16 = cpu.MemRead16(address);
            uint _temp32 = (uint)(cpu.D_REG - _temp16);

            cpu.CC_C = (_temp32 & 0x10000) >> 16 != 0;
            cpu.CC_V = OVERFLOW16(cpu.CC_C, _temp32, _temp16, cpu.D_REG);

            cpu.D_REG = (ushort)_temp32;

            cpu.CC_Z = ZTEST(cpu.D_REG);
            cpu.CC_N = NTEST16(cpu.D_REG);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 6);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._64);
    }
}