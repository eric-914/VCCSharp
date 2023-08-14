﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //MUL
    //Unsigned multiply (A x B —> D)
    public class _3D_Mul_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            cpu.D_REG = (ushort)(cpu.A_REG * cpu.B_REG);
            cpu.CC_C = cpu.B_REG > 0x7F;
            cpu.CC_Z = ZTEST(cpu.D_REG);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 11);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._1110);
    }
}
