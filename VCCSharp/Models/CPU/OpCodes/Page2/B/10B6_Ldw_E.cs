﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2.B
{
    // --[HITACHI]--
    //LDW
    //EXTENDED
    public class _10B6_Ldw_E : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.MemRead16(cpu.PC_REG);

            cpu.W_REG = cpu.MemRead16(address);

            cpu.CC_Z = ZTEST(cpu.W_REG);
            cpu.CC_N = NTEST16(cpu.W_REG);
            cpu.CC_V = false;

            cpu.PC_REG += 2;

            return Cycles._76;
        }
    }
}