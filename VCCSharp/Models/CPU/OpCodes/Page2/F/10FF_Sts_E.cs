﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2.F
{
    //STS
    //Store stack pointer to memory
    //EXTENDED
    public class _10FF_Sts_E : OpCode, IOpCode
    {
        public static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.MemRead16(cpu.PC_REG);

            cpu.MemWrite16(cpu.S_REG, address);
            
            cpu.CC_Z = ZTEST(cpu.S_REG);
            cpu.CC_N = NTEST16(cpu.S_REG);
            cpu.CC_V = false;

            cpu.PC_REG += 2;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 7);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._76);
    }
}