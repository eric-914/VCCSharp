using VCCSharp.Enums;

namespace VCCSharp.Modules.TCC1014.Masks;

public class MemorySizeStates : Dictionary<MemorySizes, byte>
{
    public MemorySizeStates()
    {
        //TODO: Verify
        Add(MemorySizes._4K, 8);
        Add(MemorySizes._16K, 8);
        Add(MemorySizes._32K, 8);
        Add(MemorySizes._64K, 8);

        //--Originally just contained these
        Add(MemorySizes._128K, 8);
        Add(MemorySizes._512K, 56);
        Add(MemorySizes._2048K, 56);
        Add(MemorySizes._8192K, 56);
    }
}
