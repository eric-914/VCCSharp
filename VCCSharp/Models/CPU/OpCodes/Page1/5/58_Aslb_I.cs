﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// ASLB
    /// Arithmetic shift of accumulator or memory 
    /// Arithmetic Shift Left of 8-Bit Accumulator or Memory Byte
    /// LSL
    /// Logical shift left accumulator or memory location
    /// INHERENT
    /// □  ←  □□□□□□□□ ← 0
    /// C bit 7      0
    /// SOURCE FORM   ADDRESSING MODE     OPCODE       CYCLES      BYTE COUNT
    /// ASLB          INHERENT            58           2 / 1       1
    ///   [E F H I N Z V C]
    ///   [    ~   ↕ ↕ ↕ ↕]
    /// </summary>
    /// <remarks>
    /// These instructions shift the contents of the A or B accumulator or a specified byte in memory to the left by one bit, clearing bit 0. 
    /// Bit 7 is shifted into the Carry flag of the Condition Codes register.
    ///     H The affect on the Half-Carry flag is undefined for these instructions.
    ///     N The Negative flag is set equal to the new value of bit 7; previously bit 6.
    ///     Z The Zero flag is set if the new 8-bit value is zero; cleared otherwise.
    ///     V The Overflow flag is set to the Exclusive-OR of the original values of bits 6 and 7.
    ///     C The Carry flag receives the value shifted out of bit 7.
    ///     
    /// The ASL instruction can be used for simple multiplication (a single left-shift multiplies the value by 2). 
    /// Other uses include conversion of data from serial to parallel and viseversa.
    /// 
    /// The 6309 does not provide variants of ASL to operate on the E and F accumulators.
    /// However, you can achieve the same functionality using the ADDR instruction. 
    /// The instructions ADDR E,E and ADDR F,F will perform the same left-shift operation on the E and F accumulators respectively.
    /// The ASL and LSL mnemonics are duplicates. Both produce the same object code.
    /// 
    /// See Also: ASLD
    /// </remarks>    
    public class _58_Aslb_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            cpu.CC_C = cpu.B_REG > 0x7F;
            cpu.CC_V = cpu.CC_C ^ ((cpu.B_REG & 0x40) >> 6 != 0);

            cpu.B_REG = (byte)(cpu.B_REG << 1);

            cpu.CC_N = NTEST8(cpu.B_REG);
            cpu.CC_Z = ZTEST(cpu.B_REG);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 2);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._21);
    }
}
