﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3.A
{
    //CMPS
    //Compare memory from stack pointer
    //INDEXED
    public class _11AC_Cmps_X : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.INDADDRESS(cpu.PC_REG++);
            ushort value = cpu.MemRead16(address);
            ushort compare = (ushort)(cpu.S_REG - value);

            cpu.CC_C = compare > cpu.S_REG;
            cpu.CC_V = OVERFLOW16(cpu.CC_C, value, compare, cpu.S_REG);
            cpu.CC_N = NTEST16(compare);
            cpu.CC_Z = ZTEST(compare);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 7);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._76);
    }
}