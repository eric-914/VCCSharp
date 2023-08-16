using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// CWAI
    /// AND condition code register, then wait for interrupt
    /// Clear Condition Code Bits and Wait for Interrupt
    /// INHERENT
    /// CC’ ← CC AND IMM
    /// CC’ ← CC OR 80(16) (E flag)
    /// Push Onto S Stack: PC,U,Y,X,DP,[WIf NM = 1],D,CC
    /// Halt Execution and Wait for Unmasked Interrupt
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// CWAI #i8        IMMEDIATE           3C          22 / 20     2
    /// 
    /// I8 : 8-bit Immediate value
    /// </summary>
    /// <remarks>
    /// This instruction logically ANDs the contents of the Condition Codes register with the 8-bit value specified by the immediate operand. 
    /// The result is placed back into the Condition Codes register. 
    /// The E flag in the CC register is then set and the entire machine state is pushed onto the hardware stack (S). 
    /// The CPU then halts execution and waits for an unmasked interrupt to occur. 
    /// When such an interrupt occurs, the CPU resumes execution at the address obtained from the corresponding interrupt vector.
    /// 
    /// You can specify a value in the immediate operand to clear either or both the I and F interrupt masks to ensure that the desired interrupt types are enabled. 
    /// One of the following values is typically used for the immediate operand:
    ///         $FF = Leave CC unmodified
    ///         $EF = Enable IRQ
    ///         $BF = Enable FIRQ
    ///         $AF = Enable both IRQ and FIRQ
    /// 
    /// Some assemblers will accept a comma-delimited list of the Condition Code bits to be cleared as an alternative to the immediate value. 
    /// For example:
    ///         CWAI I,F ; Clear I and F, wait for interrupt
    ///         
    /// It is important to note that because the entire machine state is stacked prior to the actual occurrence of an interrupt, any FIRQ service routine that may be invoked must not assume that PC and CC are the only registers that have been stacked. 
    /// The RTI instruction will operate correctly in this situation because CWAI sets the E flag prior to stacking the CC register.
    /// 
    /// Unlike SYNC, the CWAI instruction does not place the data and address busses in a high-impedance state while waiting for an interrupt.
    /// 
    /// See Also: ANDCC, RTI, SYNC
    /// </remarks>
    public class _3C_Cwai_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            var _postByte = cpu.MemRead8(cpu.PC_REG++);

            cpu.CC &= _postByte;

            cpu.IsSyncWaiting = true;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, cpu.SyncCycle);
        public int Exec(IHD6309 cpu)=> Exec(cpu, cpu.SyncCycle);
    }
}
