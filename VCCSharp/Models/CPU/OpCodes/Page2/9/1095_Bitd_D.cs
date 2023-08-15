﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// BITD
    /// 🚫 6309 ONLY 🚫
    /// Bit Test Accumulator D with Memory Word Value
    /// DIRECT
    /// TEMP ← ACCD AND (M:M+1)
    /// SOURCE FORM   ADDRESSING MODE   OPCODE      CYCLES      BYTE COUNT
    /// BITD          DIRECT            1095        7 / 5       3 
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ 0  ]
    /// </summary>
    /// <remarks>
    /// The BITD instruction logically ANDs the contents of a double-byte value in memory with the contents of Accumulator D. 
    /// The 16-bit result is tested to set or clear the appropriate flags in the CC register. 
    /// Neither Accumulator D nor the memory bytes are modified.
    ///     N The Negative flag is set equal to bit 15 of the resulting value.
    ///     Z The Zero flag is set if the resulting value was zero; cleared otherwise.
    ///     V The Overflow flag is cleared by this instruction.
    ///     C The Carry flag is not affected by this instruction.
    /// 
    /// The BITD instruction differs from ANDD only in that Accumulator D is not modified.
    /// 
    /// See Also: ANDD, BIT (8-bit), BITMD
    /// </remarks>
    public class _1095_Bitd_D : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);
            ushort value = cpu.MemRead16(address);
            ushort mask = (ushort)(cpu.D_REG & value);

            cpu.CC_N = NTEST16(mask);
            cpu.CC_Z = ZTEST(mask);
            cpu.CC_V = false;

            return Cycles._75;
        }
    }
}
