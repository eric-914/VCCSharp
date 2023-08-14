﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //LBSR
    //Branch to subroutine 
    //RELATIVE
    internal class _17_Lbsr_R : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort word = cpu.MemRead16(cpu.PC_REG);

            cpu.PC_REG += 2;
            cpu.S_REG--;

            cpu.MemWrite8(cpu.PC_L, cpu.S_REG--);
            cpu.MemWrite8(cpu.PC_H, cpu.S_REG);

            cpu.PC_REG += word;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 9);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._97);
    }
}
