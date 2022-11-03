namespace VCCSharp.Modules.TCC1014;

/// <summary>
/// MC1 MC0  ROM mapping
///  0   X   16K Internal, 16K External
///  1   0   32K Internal
///  1   1   32K External (except for vectors)
/// </summary>
public enum RomMapping : byte
{
    /* 0x */ _16kInternal_16kExternal = 0,
    /* 0x */ _16kInternal_16kExternal_Alt = 1,
    /* 10 */ _32kInternal = 2,
    /* 11 */ _32kExternal = 3
}