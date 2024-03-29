﻿using VCCSharp.OpCodes.Model.OpCodes;
using VCCSharp.OpCodes.Model.Support;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>113B/TFM2/IMMEDIATE</code>
/// Transfer Memory
/// <code>TFM r0-, r1-</code>
/// </summary>
/// <remarks>
/// The <c>TFM2</c> instruction transfers the number of bytes specified in the W accumulator from a source address pointed to by the X, Y, U, S or D registers to a destination address also pointed to by one of those registers. 
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// After each byte is transferred the source and destination registers may both be incremented by one, both decremented by one, only the source incremented, or only the destination incremented. 
/// Accumulator W is always decremented by one after each byte is transferred. 
/// The instruction completes when W is decremented to 0.
/// 
/// The forms which increment or decrement both addresses provide a block-move operation. 
/// Typically, the decrementing form is needed when the source block resides at a lower address than the destination block AND the two blocks may overlap each other.
/// 
/// The forms which increment only one of the addresses are useful for filling a block of memory with a particular byte value (destination increments), and for reading or writing a block of data from or to a memory-mapped I/O device. 
/// For the reasons described below, I/O transfers should always be performed with interrupts masked.
/// 
/// The Immediate operand for this instruction is a post-byte which uses the same format as that used by the TFR and EXG instructions. 
/// An Illegal Instruction exception will occur if the post-byte contains encodings for registers other than X, Y, U, S or D.
/// 
/// ***IMPORTANT:***
/// The TFM instructions are unique in that they are the only instructions that may be interrupted before they have completed. 
/// If an unmasked interrupt occurs while executing a TFM instruction, the CPU will interrupt the operation at a point where it has read a byte from the source address, but before it has incremented or decremented any registers or stored the byte at the destination address. 
/// The interrupt service routine will be invoked in the normal manner except for the fact that the PC value pushed onto the stack will still point to the TFM instruction. 
/// This causes the TFM instruction to be executed again when the service routine returns. 
/// Since the address registers were not updated prior to the invocation of the service routine, TFM will start by reading a byte from the previous source address for a second time.
/// 
/// It is also important to remember that in emulation mode (NM=0), the W register is not automatically preserved. 
/// If a service routine modifies W but does not explicitly preserve its original value, it could alter the actual number of bytes processed by a TFM instruction.
/// 
/// TFM r0-, r1-
/// Cycles (6 + 3n)
/// Byte Count (3)
/// (Three additional cycles are used for each BYTE transferred.)
internal class _1139_Tfm2_M : OpCode6309, IOpCode, ITransferMemory
{
    private readonly TransferMemory _tm = new();

    public int CycleCount => 6;

    public void Exec()
    {
        Cycles = _tm.Exec(this);
    }

    public void Swap(byte source, byte destination)
    {
        R16[destination] = (ushort)(R16[destination] - 1);
        R16[source] = (ushort)(R16[source] - 1);
    }
}
