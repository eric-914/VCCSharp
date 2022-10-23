using VCCSharp.Enums;

namespace VCCSharp.Modules.TCC1014.Masks;

public class VectorMasks : Dictionary<MemorySizes, byte>
{
    public VectorMasks()
    {
        //TODO: Verify
        Add(MemorySizes._4K, 15);
        Add(MemorySizes._16K, 15);
        Add(MemorySizes._32K, 15);
        Add(MemorySizes._64K, 15);

        //--Originally contained just these
        Add(MemorySizes._128K, 15);
        Add(MemorySizes._512K, 63);
        Add(MemorySizes._2048K, 63);
        Add(MemorySizes._8192K, 63);
    }
}
