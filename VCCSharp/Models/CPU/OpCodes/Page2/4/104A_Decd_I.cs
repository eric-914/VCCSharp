﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    public class _104A_Decd_I : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            cpu.D_REG--;
            cpu.CC_Z = ZTEST(cpu.D_REG);
            cpu.CC_V = cpu.D_REG == 0x7FFF;
            cpu.CC_N = NTEST16(cpu.D_REG);

            return Cycles._32;
        }
    }
}
