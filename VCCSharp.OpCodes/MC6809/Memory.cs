using VCCSharp.OpCodes.Model.Memory;
using VCCSharp.OpCodes.Model.Support;

namespace VCCSharp.OpCodes.MC6809;

internal class Memory
{
    public Memory(IMemory cpu, IExtendedAddress ea)
    {
        Byte = new Memory8Bit(cpu);
        Word = new Memory16Bit(cpu);
        DP = new MemoryDirect(cpu);
        Indexed = new MemoryIndexed(cpu, ea);
    }

    public Memory8Bit Byte { get; }
    public Memory16Bit Word { get; }
    public MemoryDirect DP { get; }
    public MemoryIndexed Indexed { get; }
}
