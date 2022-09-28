using VCCSharp.Enums;

namespace VCCSharp.Modules.TC1014.Masks;

public class MemoryConfigurations : Dictionary<MemorySizes, uint>
{
    public MemoryConfigurations()
    {
        //TODO: Verify
        Add(MemorySizes._4K, 0x01000);
        Add(MemorySizes._16K, 0x04000);
        Add(MemorySizes._32K, 0x08000);
        Add(MemorySizes._64K, 0x10000);

        //--Originally just contained these
        Add(MemorySizes._128K, 0x20000);
        Add(MemorySizes._512K, 0x80000);
        Add(MemorySizes._2048K, 0x200000);
        Add(MemorySizes._8192K, 0x800000);
    }
}
