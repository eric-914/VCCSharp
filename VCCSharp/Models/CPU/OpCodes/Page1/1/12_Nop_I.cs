using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// NOP
    /// No operation
    /// INHERENT
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// NOP             INHERENT            12          2 / 1       1
    ///   [E F H I N Z V C]
    ///   [               ]
    /// </summary>
    /// <remarks>
    /// The NOP instruction advances the Program Counter by one byte without affecting any other registers or condition codes.
    /// 
    /// The NOP instruction provides a single-byte no-op that consumes two bus cycles (one cycle on a 6309 when NM=1). 
    /// Some larger, more time-consuming instructions that can also be used as effective no-ops include:
    ///         BRN             LBRN
    ///         ANDCC #$FF      ORCC #0
    ///         PSHS #0         PULS #0
    ///         PSHU #0         PULU #0
    ///         EXG r,r         TFR r,r
    ///         LEAS ,S         LEAS ,S+        LEAS ,S++
    ///         LEAU ,U         LEAU ,U+        LEAU ,U++
    ///         
    /// See Also: BRN, EXG, LBRN, LEA, PSH, PUL, TFR
    /// </remarks>
    public class _12_Nop_I : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu)
        {
            return 2;
        }

        public int Exec(IHD6309 cpu)
        {
            return Cycles._21;
        }
    }
}
