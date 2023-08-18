using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// ORCC
    /// OR condition code register 
    /// Logically OR the CC Register with an Immediate Value
    /// IMMEDIATE
    /// CC’ ← CC OR IMM8
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// ORCC #i8        IMMEDIATE           1A          3           2
    /// 
    /// I8 : 8-bit Immediate value
    /// </summary>
    /// <remarks>
    /// This instruction logically ORs the contents of the Condition Codes register with the 8-bit immediate value specified in the operand. 
    /// The result is placed back into the Condition Codes register.
    /// 
    /// The ORCC instruction provides a method to set specific flags in the Condition Codes register. 
    /// All flags that correspond to '1' bits in the immediate operand are set, while those corresponding with '0's are left unchanged.
    /// 
    /// The bit numbers for each flag are shown below:
    ///     7 6 5 4 3 2 1 0
    ///     E F H I N Z V C
    ///     
    /// One of the more common uses for the ORCC instruction is to set the IRQ and FIRQ Interrupt Masks (I and F) at the beginning of a routine that must run with interrupts disabled. 
    /// This is accomplished by executing:
    ///         ORCC #$50 ; Set bits 4 and 6 in CC
    ///         
    /// Some assemblers will accept a comma-delimited list of the bit names as an alternative to the immediate value. 
    /// For instance, the example above might also be written as:
    ///         ORCC I,F  ; Set bits 4 and 6 in CC
    ///         
    /// More examples:
    ///         ORCC #1   ; Set the Carry flag
    ///         ORCC #$80 ; Set the Entire flag
    ///         
    /// See Also: ANDCC, OR (8-bit), ORD, ORR
    /// </remarks>
    public class _1A_Orcc_M : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            byte value = cpu.MemRead8(cpu.PC_REG++);
            byte mask = cpu.CC;

            mask = (byte)(mask | value);

            cpu.CC = mask;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 3);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._32);
    }
}
