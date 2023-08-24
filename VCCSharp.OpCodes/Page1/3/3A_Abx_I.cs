using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>3A/ABX/INHERENT</code>
/// Add Accumulator <c>B</c> to Index Register <c>X</c> (unsigned)
/// <code>X’ ← X + ACCB</code>
/// </summary>
/// <remarks>
/// The <c>ABX</c> instruction performs an unsigned addition of the contents of Accumulator <c>B</c> with the contents of Index Register <c>X</c>.
/// The 16-bit result is placed into Index Register <c>X</c>.
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected.
/// 
/// The ABX instruction is similar in function to the LEAX B,X instruction. 
/// A significant difference is that LEAX B,X treats B as a twos complement value (signed), whereas ABX treats B as unsigned. 
/// For example, if X were to contain 0x301B and B were to contain 0xFF, then ABX would produce 0x311A in X, whereas LEAX B,X would produce 0x301A in X.
/// 
/// Additionally, the ABX instruction does not affect any flags in the Condition Codes register, whereas the LEAX instruction does affect the Zero flag.
/// 
/// One example of a situation where the ABX instruction may be used is when X contains the base address of a data structure or array and B contains an offset to a specific field or array element. 
/// In this scenario, ABX will modify X to point directly to the field or array element.
/// 
/// The ABX instruction was included in the 6x09 instruction set for compatibility with the 6801 microprocessor.
/// 
/// Cycles (3 / 1)
/// Byte Count (1)
/// 
internal class _3A_Abx_I : OpCode, IOpCode
{
    internal _3A_Abx_I(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        X += B;

        return Cycles._31;
    }
}
