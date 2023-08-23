namespace VCCSharp.OpCodes.Definitions;

/// <summary>
/// Use by the cycles calculation.  Some opcodes execute faster on the 6309.
/// </summary>
public enum Mode
{
    MC6809 = 0,
    HD6309 = 1
}
