﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    //INHERENT
    public class _105F_Clrw_I : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            cpu.W_REG = 0;
            cpu.CC_C = false;
            cpu.CC_V = false;
            cpu.CC_N = false;
            cpu.CC_Z = true; 

            return Cycles._32;
        }
    }
}
