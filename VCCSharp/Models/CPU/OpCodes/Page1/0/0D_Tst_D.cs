using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// TST
    /// Test accumulator or memory location 
    /// Test Value in Memory Byte
    /// DIRECT
    /// TEMP ← (M)
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// TST             DIRECT              0D          6 / 4       2 
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ 0  ]
    /// </summary>
    /// <remarks>
    /// The TST instructions test the value in a memory byte to setup the Condition Codes register with minimal status for that value. 
    /// The memory byte is not modified.
    ///         N The Negative flag is set equal to bit 7 of the byte’s value (sign bit).
    ///         Z The Zero flag is set if the byte’s value is zero; cleared otherwise.
    ///         V The Overflow flag is always cleared.
    ///         C The Carry flag is not affected by this instruction.
    ///         
    /// For unsigned values, the only meaningful information provided is whether or not the value is zero. 
    /// In this case, BEQ or BNE would typically follow such a test.
    /// 
    /// For signed (twos complement) values, the information provided is sufficient to allow any of the signed conditional branches (BGE, BGT, BLE, BLT) to be used as though the byte’s value had been compared with zero. 
    /// You could also use BMI and BPL to branch according to the sign of the value.
    /// 
    /// You can obtain the same information in fewer cycles by loading the byte into an 8-bit accumulator (LDA and LDB are fastest). For this reason it is usually preferable to avoid using TST on a memory byte if there is an available accumulator.
    /// 
    /// See Also: CMP, LD (8-bit),TST (accumulator)
    /// </remarks>
    public class _0D_Tst_D : OpCode, IOpCode
    {
        public static int Exec(ICpuProcessor cpu, int cycles)
        {
            var address = cpu.DPADDRESS(cpu.PC_REG++);
            var _temp8 = cpu.MemRead8(address);

            cpu.CC_Z = ZTEST(_temp8);
            cpu.CC_N = NTEST8(_temp8);
            cpu.CC_V = false;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 6);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._64);
    }
}
