using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>7D/TST/EXTENDED</code>
/// Test Value in Memory Byte
/// <code>TEMP ← (M)</code>
/// </summary>
/// <remarks>
/// The <c>TST</c> instructions test the value in a memory byte to setup the Condition Codes register with minimal status for that value. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0  ]
/// 
/// The memory byte is not modified.
///         N The Negative flag is set equal to bit 7 of the byte’s value (sign bit).
///         Z The Zero flag is set if the byte’s value is zero; cleared otherwise.
///         V The Overflow flag is always cleared.
///         
/// For unsigned values, the only meaningful information provided is whether or not the value is zero. 
/// In this case, BEQ or BNE would typically follow such a test.
/// 
/// For signed (twos complement) values, the information provided is sufficient to allow any of the signed conditional branches (BGE, BGT, BLE, BLT) to be used as though the byte’s value had been compared with zero. 
/// You could also use BMI and BPL to branch according to the sign of the value.
/// 
/// You can obtain the same information in fewer cycles by loading the byte into an 8-bit accumulator (LDA and LDB are fastest). For this reason it is usually preferable to avoid using TST on a memory byte if there is an available accumulator.
/// 
/// Cycles (7 / 5)
/// Byte Count (3)
/// 
/// See Also: CMP, LD (8-bit),TST (accumulator)
internal class _7D_Tst_E : OpCode, IOpCode
{
    internal _7D_Tst_E(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        ushort address = M16[PC+=2];
        byte value = M8[address];

        CC_N = value.Bit7();
        CC_Z = value == 0;
        CC_V = false;

        return DynamicCycles._75;
    }
}
