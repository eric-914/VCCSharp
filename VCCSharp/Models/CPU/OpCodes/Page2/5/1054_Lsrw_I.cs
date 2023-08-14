﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    // --[HITACHI]--
    //LSRW
    //INHERENT
    public class _1054_Lsrw_I : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            cpu.CC_C = (cpu.W_REG & 1) != 0;
            cpu.W_REG = (ushort)(cpu.W_REG >> 1);
            cpu.CC_Z = ZTEST(cpu.W_REG);
            cpu.CC_N = false;

            return Cycles._32;
        }
    }
}