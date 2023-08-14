﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //NEGA
    //Negate accumulator or memory
    public class _40_Nega_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            var _temp8 = (byte)(0 - cpu.A_REG);

            cpu.CC_C = _temp8 > 0;
            cpu.CC_V = cpu.A_REG == 0x80; //CC_C ^ ((A_REG^temp8)>>7);
            cpu.CC_N = NTEST8(_temp8);
            cpu.CC_Z = ZTEST(_temp8);

            cpu.A_REG = _temp8;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 2);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._21);
    }
}
