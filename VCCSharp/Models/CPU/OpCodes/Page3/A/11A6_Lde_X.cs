﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3.A
{
    // --[HITACHI]--
    //LDE
    //INDEXED
    public class _11A6_Lde_X : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.INDADDRESS(cpu.PC_REG++);

            cpu.E_REG = cpu.MemRead8(address);
            cpu.CC_Z = ZTEST(cpu.E_REG);
            cpu.CC_N = NTEST8(cpu.E_REG);
            cpu.CC_V = false;

            return 5;
        }
    }
}