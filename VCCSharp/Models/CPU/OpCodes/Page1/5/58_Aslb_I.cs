﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //ASL
    //Arithmetic shift of accumulator or memory 
    //INHERENT
    public class _58_Aslb_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            cpu.CC_C = cpu.B_REG > 0x7F;
            cpu.CC_V = cpu.CC_C ^ ((cpu.B_REG & 0x40) >> 6 != 0);

            cpu.B_REG = (byte)(cpu.B_REG << 1);

            cpu.CC_N = NTEST8(cpu.B_REG);
            cpu.CC_Z = ZTEST(cpu.B_REG);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 2);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._21);
    }
}
