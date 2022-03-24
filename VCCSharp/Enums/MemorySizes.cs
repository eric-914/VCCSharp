namespace VCCSharp.Enums
{
    public enum MemorySizes
    {
        //TODO: Reorder once we know for sure not using byte values of this anywhere.
        _4K = -4,
        _16K = -3,
        _32K = -2,
        _64K = -1,

        _128K = 0,
        _512K = 1,
        _2048K = 2,
        _8192K = 3
    }
}
