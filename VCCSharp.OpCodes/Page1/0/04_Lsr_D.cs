﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>04/LSR/DIRECT</code>
/// Logical Shift Right of 8-Bit memory byte
/// <code>0 → b7 → ... → b0 → C</code>
/// </summary>
/// <remarks>
/// These instructions logically shift the contents of the specified byte in memory to the right by one bit, clearing bit 7. 
/// </remarks>
/// 
///          ╭──┬──┬──┬──┬──┬──┬──┬──╮     ╭──╮
///    0 ──▶ │  │  │  │  │  │  │  │  │ ──▶ │  │
///          ╰──┴──┴──┴──┴──┴──┴──┴──╯     ╰──╯
///           b7 ────────────────▶ b0       C
///           
/// [E F H I N Z V C]
/// [        0 ↕   ↕]
/// 
/// These instructions logically shift the contents of the specified byte in memory to the right by one bit, clearing bit 7. 
/// Bit 0 is shifted into the Carry flag of the Condition Codes register.
///         N The Negative flag is cleared by these instructions.
///         Z The Zero flag is set if the new 8-bit value is zero; cleared otherwise.
///         C The Carry flag receives the value shifted out of bit 0.
///        
/// The LSR instruction can be used in simple division routines on unsigned values (a single right-shift divides the value by 2).
/// 
/// Cycles (6 / 5)
/// Byte Count (2)
/// 
/// See Also: LSR (16-bit)
internal class _04_Lsr_D : OpCode, IOpCode
{
    internal _04_Lsr_D(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        ushort address = DIRECT[PC++];
        byte value = M8[address];

        byte result = (byte)(value >> 1);

        CC_N = false;
        CC_Z = result == 0;
        CC_C = value.Bit0();

        M8[address] = result;

        return Cycles._65;
    }
}
