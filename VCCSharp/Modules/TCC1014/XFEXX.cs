namespace VCCSharp.Modules.TCC1014;

// ReSharper disable InconsistentNaming
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
/// <summary>
/// FF90: Initialization Register 0 (INIT0)
/// BIT 3 = MC3     1 = DRAM at XFEXX is constant
/// When 1: $FE00-$FEFF = vector addresses
/// When 0: $FE00-$FEFF = RAM addresses
/// </summary>
/// <remarks>
/// TODO: Come up with a better name?
/// COCO-3 specific
/// </remarks>
public enum XFEXX : byte
{
    RAM = 0,    // 000
    Vector = 8  // 100
}
