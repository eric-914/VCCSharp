﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3.B
{
    //CMPS
    //Compare memory from stack pointer
    //EXTENDED
    public class _11BC_Cmps_E : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.MemRead16(cpu.PC_REG);
            ushort value = cpu.MemRead16(address);
            ushort compare = (ushort)(cpu.S_REG - value);

            cpu.CC_C = compare > cpu.S_REG;
            cpu.CC_V = OVERFLOW16(cpu.CC_C, value, compare, cpu.S_REG);
            cpu.CC_N = NTEST16(compare);
            cpu.CC_Z = ZTEST(compare);
            cpu.PC_REG += 2;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 8);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._86);
    }
}