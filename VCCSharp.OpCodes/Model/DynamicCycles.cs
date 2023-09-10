using VCCSharp.Models.CPU.OpCodes;
using VCCSharp.OpCodes.Definitions;

namespace VCCSharp.OpCodes.Model;

/// <summary>
/// Some opcodes are executed faster on the HD6309 when it is running in native mode.
/// This lookup allows a mechanism to choose the appropriate cycle count.
/// The MC6809 will always be the the longer count.
/// The HD6309 will depend on if its running in native mode vs. emulation mode. (Mode ⇔ MD_NATIVE6309)
/// </summary>
internal class DynamicCycles
{
    private static byte[,] _cycles =
    {
        {6, 5},  
        {6, 4},  
        {3, 2},  
        {2, 1},  
        {5, 4},  
        {9, 7},  
        {8, 5},  
        {5, 1},  
        {3, 1},  
        {11, 10},
        {7, 6},  
        {7, 5},  
        {4, 3},  
        {8, 7},  
        {8, 6},  
        {9, 8},  
        {27, 26},
        {36, 35},
        {30, 29},
        {28, 27},
        {37, 36},
        {31, 30},
        {4, 2},  
        {5, 3}   
    };

    private byte T(int index) => _cycles[index, (int)_mode()];

    #region Factory

    public byte _65 => T(0);
    public byte _64 => T(1);
    public byte _32 => T(2);
    public byte _21 => T(3);
    public byte _54 => T(4);
    public byte _97 => T(5);
    public byte _85 => T(6);
    public byte _51 => T(7);
    public byte _31 => T(8);
    public byte _1110 => T(9);
    public byte _76 => T(10);
    public byte _75 => T(11);
    public byte _43 => T(12);
    public byte _87 => T(13);
    public byte _86 => T(14);
    public byte _98 => T(15);
    public byte _2726 => T(16);
    public byte _3635 => T(17);
    public byte _3029 => T(18);
    public byte _2827 => T(19);
    public byte _3736 => T(20);
    public byte _3130 => T(21);
    public byte _42 => T(22);
    public byte _53 => T(23);

    #endregion

    private readonly Func<Mode> _mode;

    public DynamicCycles(Func<Mode> mode)
    {
        _mode = mode;
    }
}
