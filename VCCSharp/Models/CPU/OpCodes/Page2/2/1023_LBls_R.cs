﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    //LBLS
    //Branch if lower or same (unsigned)
    //RELATIVE
    public class _1023_LBls_R : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            if (cpu.CC_C | cpu.CC_Z)
            {
                cpu.PC_REG += (ushort)(short)cpu.MemRead16(cpu.PC_REG);

                cycles += 1;
            }

            cpu.PC_REG += 2;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 5);
        public int Exec(IHD6309 cpu) => Exec(cpu, 5);
    }
}