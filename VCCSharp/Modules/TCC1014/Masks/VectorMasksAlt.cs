using VCCSharp.Enums;

namespace VCCSharp.Modules.TCC1014.Masks;

public class VectorMasksAlt : Dictionary<MemorySizes, byte>
{
    public VectorMasksAlt()
    {
        //TODO: Verify
        Add(MemorySizes._4K, 12);
        Add(MemorySizes._16K, 12);
        Add(MemorySizes._32K, 12);
        Add(MemorySizes._64K, 12);

        //--Originally contained just these
        Add(MemorySizes._128K, 12);
        Add(MemorySizes._512K, 60);
        Add(MemorySizes._2048K, 60);
        Add(MemorySizes._8192K, 60);
    }
}
