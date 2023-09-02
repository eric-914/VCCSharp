using VCCSharp.Models.CPU.OpCodes;

namespace VCCSharp.OpCodes.Model;

/// <summary>
/// Some opcodes are executed faster on the HD6309 when it is running in native mode.
/// This lookup allows a mechanism to choose the appropriate cycle count.
/// The MC6809 will always be the the longer count.
/// The HD6309 will depend on if its running in native mode vs. emulation mode. (Mode ⇔ MD_NATIVE6309)
/// </summary>
internal class DynamicCycles
{
    private readonly IMode _cpu;

    private static byte[,] _cycles =
    {
        {6, 5},    /* M65 */
        {6, 4},    /* M64 */
        {3, 2},    /* M32 */
        {2, 1},    /* M21 */
        {5, 4},    /* M54 */
        {9, 7},    /* M97 */
        {8, 5},    /* M85 */
        {5, 1},    /* M51 */
        {3, 1},    /* M31 */
        {11, 10},  /* M1110 */
        {7, 6},    /* M76 */
        {7, 5},    /* M75 */
        {4, 3},    /* M43 */
        {8, 7},    /* M87 */
        {8, 6},    /* M86 */
        {9, 8},    /* M98 */
        {27, 26},  /* M2726 */
        {36, 35},  /* M3635 */
        {30, 29},  /* M3029 */
        {28, 27},  /* M2827 */
        {37, 26},  /* M3726 */
        {31, 30},  /* M3130 */
        {4, 2},    /* M42 */
        {5, 3}     /* M53 */
    };

    private byte T(int index) => _cycles[index, (int)_cpu.Mode];

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
    public byte _3726 => T(20);
    public byte _3130 => T(21);
    public byte _42 => T(22);
    public byte _53 => T(23);

    #endregion

    public DynamicCycles(IMode cpu)
    {
        _cpu = cpu;
    }
}
