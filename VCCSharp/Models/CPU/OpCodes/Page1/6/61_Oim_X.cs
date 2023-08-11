﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    public class _61_Oim_X : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            byte value = cpu.MemRead8(cpu.PC_REG++);
            ushort address = cpu.INDADDRESS(cpu.PC_REG++);

            value |= cpu.MemRead8(address);

            cpu.MemWrite8(value, address);
            cpu.CC_N = NTEST8(value);
            cpu.CC_Z = ZTEST(value);
            cpu.CC_V = false;

            return 6;
        }
    }
}