using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>7F/CLR/EXTENDED</code>
/// Store Zero into a Memory byte
/// <code>(M) ← 0</code>
/// </summary>
/// <remarks>
/// This instruction clears (sets to zero) the byte in memory at the Effective Address specified by the operand. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        0 1 0 0]
/// 
/// The Condition Code flags are also modified as follows:
///         N The Negative flag is cleared.
///         Z The Zero flag is set.
///         V The Overflow flag is cleared.
///         C The Carry flag is cleared.
///         
/// The CPU performs a Read-Modify-Write sequence when this instruction is executed and is therefore slower than an instruction which only writes to memory. 
/// When more than one byte needs to be cleared, you can optimize for speed by first clearing an accumulator and then using ST instructions to clear the memory bytes. 
/// The following examples illustrate this optimization:
/// 
/// Executes in 21 cycles (NM=0):
///         CLR $200 ; 7 cycles
///         CLR $210 ; 7 cycles
///         CLR $220 ; 7 cycles
///         
/// Adds one additional code byte, but saves 4 cycles:
///         CLRA ; 2 cycles
///         STA $200 ; 5 cycles
///         STA $210 ; 5 cycles
///         STA $220 ; 5 cycles
///         
/// Cycles (7 / 6)
/// Byte Count (3)
/// 
/// See Also: CLR (accumulator), ST
internal class _7F_Clr_E : OpCode, IOpCode
{
    public int CycleCount => DynamicCycles._76;

    public int Exec()
    {
        ushort address = M16[PC]; PC += 2;

        M8[address] = 0;

        CC_N = false;
        CC_Z = true;
        CC_V = false;
        CC_C = false;

        return CycleCount;
    }
}
