﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2.A
{
    // --[HITACHI]--
    //CMPW
    //INDEXED
    public class _10A1_Cmpw_X : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.INDADDRESS(cpu.PC_REG++);
            ushort _postWord = cpu.MemRead16(address);
            ushort _temp16 = (ushort)(cpu.W_REG - _postWord);

            cpu.CC_C = _temp16 > cpu.W_REG;
            cpu.CC_V = OVERFLOW16(cpu.CC_C, _postWord, _temp16, cpu.W_REG);
            cpu.CC_N = NTEST16(_temp16);
            cpu.CC_Z = ZTEST(_temp16);

            return Cycles._76;
        }
    }
}