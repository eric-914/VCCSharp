using VCCSharp.Enums;

namespace VCCSharp.Modules.TCC1014.Masks;

public class RamMask : Dictionary<MemorySizes, ushort>
{
    public RamMask()
    {
        //TODO: Verify
        Add(MemorySizes._4K, 0);
        Add(MemorySizes._16K, 1);
        Add(MemorySizes._32K, 3);
        Add(MemorySizes._64K, 7);

        //--Originally just contained these
        Add(MemorySizes._128K, 15);
        Add(MemorySizes._512K, 63);
        Add(MemorySizes._2048K, 255);
        Add(MemorySizes._8192K, 1023);
    }
}
