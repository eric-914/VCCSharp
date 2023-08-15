﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3
{
    /// <summary>
    /// CLRF
    /// 🚫 6309 ONLY 🚫
    /// Clear accumulator or memory location
    /// Load Zero into Accumulator
    /// INHERENT
    /// r ← 0
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES     BYTE COUNT
    /// CLRW            INHERENT            105F        3 / 2      2
    /// 
    /// Each of these instructions clears (sets to zero) the specified accumulator. 
    /// The Condition Code flags are also modified as follows:
    ///     N The Negative flag is cleared.
    ///     Z The Zero flag is set.
    ///     V The Overflow flag is cleared.
    ///     C The Carry flag is cleared.
    ///     
    /// Clearing the Q accumulator can be accomplished by executing both CLRD and CLRW.
    /// 
    /// To clear any of the Index Registers (X, Y, U or S), you can use either an Immediate Mode LD instruction or, on 6309 processors only, a TFR or EXG instruction which specifies the Zero register (0) as the source.
    /// 
    /// The CLRA and CLRB instructions provide the smallest, fastest way to clear the Carry flag in the CC register.
    /// 
    /// See Also: CLR (memory), LD
    /// </summary>
    public class _115F_Clrf_I : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            cpu.F_REG = 0;

            cpu.CC_C = false;
            cpu.CC_V = false;
            cpu.CC_N = false;
            cpu.CC_Z = true;

            return Cycles._32;
        }
    }
}
