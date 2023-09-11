using VCCSharp.OpCodes.Model.Memory;
using VCCSharp.OpCodes.Model.Support;

namespace VCCSharp.OpCodes.HD6309;

internal class Memory
{
    public Memory(IMemory cpu, IExtendedAddress ea)
    {
        Byte = new Memory8Bit (cpu);
        Word = new Memory16Bit(cpu);
        DWord = new Memory32Bit (cpu);
        Direct = new MemoryDirect(cpu);
        Indexed = new MemoryIndexed (cpu, ea);
    }

    public Memory8Bit Byte { get; }
    public Memory16Bit Word { get; }
    public Memory32Bit DWord { get; }
    public MemoryDirect Direct { get; }
    public MemoryIndexed Indexed { get; }
}
