using VCCSharp.Enums;

namespace VCCSharp.Modules.TC1014.Masks;

public class VideoMasks : Dictionary<MemorySizes, uint>
{
    public VideoMasks()
    {
        //TODO: Verify
        Add(MemorySizes._4K, 0x0FFF);
        Add(MemorySizes._16K, 0x3FFF);
        Add(MemorySizes._32K, 0x7FFF);
        Add(MemorySizes._64K, 0xFFFF);

        //--Originally contained just these
        Add(MemorySizes._128K, 0x1FFFF);
        Add(MemorySizes._512K, 0x7FFFF);
        Add(MemorySizes._2048K, 0x1FFFFF);
        Add(MemorySizes._8192K, 0x7FFFFF);
    }
}
