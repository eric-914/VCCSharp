﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3.C
{
    /// <summary>
    /// SUBF
    /// 🚫 6309 ONLY 🚫
    /// Subtract memory from accumulator
    /// Subtract from value in 8-Bit Accumulator
    /// IMMEDIATE
    /// r’ ← r - IMM8|(M)
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// SUBF            IMMEDIATE           11C0        3           3 
    ///   [E F H I N Z V C]
    ///   [    ~   ↕ ↕ ↕ ↕]
    /// </summary>
    public class _11C0_Subf_M : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            byte value = cpu.MemRead8(cpu.PC_REG++);
            ushort difference = (ushort)(cpu.F_REG - value);

            cpu.CC_C = (difference & 0x100) >> 8 != 0;
            cpu.CC_V = OVERFLOW8(cpu.CC_C, value, difference, cpu.F_REG);

            cpu.F_REG = (byte)difference;

            cpu.CC_Z = ZTEST(cpu.F_REG);
            cpu.CC_N = NTEST8(cpu.F_REG);

            return 3;
        }
    }
}
