﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>73/COM/EXTENDED</code>
/// Complement a byte in Memory
/// <code>(M)’ ← ~(M)</code>
/// </summary>
/// <remarks>
/// This instruction changes the value of a byte in memory to that of it’s logical complement; that is each 1 bit is changed to a 0, and each 0 bit is changed to a 1. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0 1]
///   
/// The Condition Code flags are also modified as follows:
///         N The Negative flag is set equal to the new value of bit 7.
///         Z The Zero flag is set if the new value is zero; cleared otherwise.
///         V The Overflow flag is always cleared.
///         C The Carry flag is always set.
/// 
/// This instruction performs a ones-complement operation. 
/// A twos-complement can be achieved with the NEG instruction.
/// 
/// Cycles (7 / 6)
/// Byte Count (3)
/// 
/// See Also: COM (accumulator), NEG
internal class _73_Com_E : OpCode, IOpCode
{
    public int CycleCount => DynamicCycles._76;

    public void Exec()
    {
        ushort address = M16[PC]; PC += 2;
        byte value = M8[address];

        byte result = (byte)(0xFF - value);

        CC_Z = result == 0;
        CC_N = result.Bit7();
        CC_V = false;
        CC_C = true;

        M8[address] = result;
    }
}
