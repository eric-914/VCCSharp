﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2.B
{
    // --[HITACHI]--
    //SUBW
    //EXTENDED
    public class _10B0_Subw_E : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.MemRead16(cpu.PC_REG);
            ushort value = cpu.MemRead16(address);
            uint difference = (uint)(cpu.W_REG - value);

            cpu.CC_C = (difference & 0x10000) >> 16 != 0;
            cpu.CC_V = OVERFLOW16(cpu.CC_C, difference, value, cpu.W_REG);
            
            cpu.W_REG = (ushort)difference;
            
            cpu.CC_Z = ZTEST(cpu.W_REG);
            cpu.CC_N = NTEST16(cpu.W_REG);

            cpu.PC_REG += 2;

            return Cycles._86;
        }
    }
}