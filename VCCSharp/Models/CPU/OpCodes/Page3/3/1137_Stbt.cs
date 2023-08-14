﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3
{
    // --[HITACHI]--
    //STBT
    public class _1137_Stbt : OpCode, IOpCode
    {
        private static readonly IOpCode InvalidOpCode = new InvalidOpCode();

        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            byte value = cpu.MemRead8(cpu.PC_REG++);
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);
            byte mask = cpu.MemRead8(address);

            byte source = (byte)((value >> 3) & 7);
            byte destination = (byte)(value & 7);

            value >>= 6;

            if (value == 3)
            {
                return InvalidOpCode.Exec(cpu);
            }

            switch (value)
            {
                case 0: // A Reg
                case 1: // B Reg
                    value = cpu.PUR(value);
                    break;

                case 2: // CC Reg
                    value = cpu.CC;
                    break;
            }

            if ((value & (1 << source)) != 0)
            {
                mask |= (byte)(1 << destination);
            }
            else
            {
                mask &= (byte)~(1 << destination);
            }

            cpu.MemWrite8(mask, address);

            return Cycles._87;
        }
    }
}