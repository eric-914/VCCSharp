﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //ROL
    //Rotate accumulator or memory left
    //DIRECT
    public class _09_Rol_D : OpCode, IOpCode
    {
        public static int Exec(ICpuProcessor cpu, int cycles)
        {
            var address = cpu.DPADDRESS(cpu.PC_REG++);
            var value = cpu.MemRead8(address);
            var bit = (byte)(cpu.CC_C ? 1 : 0);

            cpu.CC_C = (value & 0x80) >> 7 != 0;
            cpu.CC_V = ((cpu.CC_C ? 1 : 0) ^ ((value & 0x40) >> 6)) != 0;
            value = (byte)((value << 1) | bit);
            cpu.CC_Z = ZTEST(value);
            cpu.CC_N = NTEST8(value);

            cpu.MemWrite8(value, address);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 6);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._65);
    }
}
