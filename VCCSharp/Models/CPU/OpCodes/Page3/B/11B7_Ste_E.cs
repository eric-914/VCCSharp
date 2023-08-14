﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3.B
{
    // --[HITACHI]--
    //STE
    //EXTENDED
    public class _11B7_Ste_E : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.MemRead16(cpu.PC_REG);

            cpu.MemWrite8(cpu.E_REG, address);

            cpu.CC_Z = ZTEST(cpu.E_REG);
            cpu.CC_N = NTEST8(cpu.E_REG);
            cpu.CC_V = false;
            
            cpu.PC_REG += 2;

            return Cycles._65;
        }
    }
}