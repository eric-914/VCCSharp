﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    //LDY
    //Load index register from memory
    //IMMEDIATE
    public class _108E_Ldy_M : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            cpu.Y_REG = cpu.MemRead16(cpu.PC_REG);
            cpu.CC_Z = ZTEST(cpu.Y_REG);
            cpu.CC_N = NTEST16(cpu.Y_REG);
            cpu.CC_V = false;

            cpu.PC_REG += 2;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 5);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._54);
    }
}