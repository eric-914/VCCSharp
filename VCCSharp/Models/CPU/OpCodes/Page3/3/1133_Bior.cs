﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3
{
    // --[HITACHI]--
    //BIOR
    public class _1133_Bior : OpCode, IOpCode
    {
        private static readonly IOpCode InvalidOpCode = new InvalidOpCode();

        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            byte value = cpu.MemRead8(cpu.PC_REG++);
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);
            byte mask = cpu.MemRead8(address);

            byte source = (byte)((value >> 3) & 7);
            byte dest = (byte)(value & 7);

            value >>= 6;

            if (value == 3)
            {
                return InvalidOpCode.Exec(cpu);
            }

            if ((mask & (1 << source)) == 0)
            {
                switch (value)
                {
                    case 0: // A Reg
                    case 1: // B Reg
                        cpu.OUR(value, dest);
                        break;

                    case 2: // CC Reg
                        cpu.CC = (byte)(cpu.CC | (1 << dest));
                        break;
                }
            }

            // Else nothing changes
            return Cycles._76;
        }
    }
}
