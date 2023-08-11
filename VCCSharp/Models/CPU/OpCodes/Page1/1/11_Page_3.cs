﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    public class _11_Page_3 : OpCode, IOpCode
    {
        public static int Exec(ICpuProcessor cpu, int cycles)
        {
            throw new NotImplementedException();

            byte opCode = cpu.MemRead8(cpu.PC_REG++);

            //TODO: Need jump vectors defined here...

            //_jumpVectors0x11[opCode]();

            //TODO: Need cycle count penalty.  Probably from Page 3 opcodes.

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 0);
        public int Exec(IHD6309 cpu) => Exec(cpu, 0);
    }
}