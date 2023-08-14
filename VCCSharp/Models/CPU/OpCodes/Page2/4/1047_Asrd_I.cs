﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    // --[HITACHI]--
    //ASRD
    //INHERENT
    public class _1047_Asrd_I : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            cpu.CC_C = (cpu.D_REG & 1) != 0;
            cpu.D_REG = (ushort)((cpu.D_REG & 0x8000) | (cpu.D_REG >> 1));
            cpu.CC_Z = ZTEST(cpu.D_REG);
            cpu.CC_N = NTEST16(cpu.D_REG);

            return Cycles._32;
        }
    }
}