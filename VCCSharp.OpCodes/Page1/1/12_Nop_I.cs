using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>12/NOP/INHERENT</code>
/// No operation
/// </summary>
/// <remarks>
/// The NOP instruction advances the Program Counter by one byte without affecting any other registers or condition codes.
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
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
/// Cycles (2 / 1)
/// Byte Count (1)
/// 
/// See Also: BRN, EXG, LBRN, LEA, PSH, PUL, TFR
internal class _12_Nop_I : OpCode, IOpCode
{
    public int CycleCount => DynamicCycles._21;

    public int Exec()
    {
        return DynamicCycles._21;
    }
}
