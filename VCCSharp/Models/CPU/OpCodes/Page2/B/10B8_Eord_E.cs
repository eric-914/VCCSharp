﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2.B
{
    //EXTENDED
    public class _10B8_Eord_E : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            var address = cpu.MemRead16(cpu.PC_REG);
            var value = cpu.MemRead16(address);

            cpu.D_REG ^= value;
            cpu.CC_N = NTEST16(cpu.D_REG);
            cpu.CC_Z = ZTEST(cpu.D_REG);
            cpu.CC_V = false;

            cpu.PC_REG += 2;

            return Cycles._86;
        }
    }
}
