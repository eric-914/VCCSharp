using VCCSharp.OpCodes.Model.Memory;
using VCCSharp.OpCodes.Model.Support;

namespace VCCSharp.OpCodes.MC6809;

internal class Memory
{
    public Memory(IMemory cpu, IExtendedAddressing ea)
    {
        Byte = new Memory8Bit(cpu);
        Word = new Memory16Bit(cpu);
        DP = new MemoryDP(cpu);
        Indexed = new MemoryIndexed(cpu, ea);
    }

    public Memory8Bit Byte { get; }
    public Memory16Bit Word { get; }
    public MemoryDP DP { get; }
    public MemoryIndexed Indexed { get; }
}
