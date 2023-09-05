﻿using VCCSharp.OpCodes.Memory;
using VCCSharp.OpCodes.Model.Memory;

namespace VCCSharp.OpCodes.HD6309;

internal class Memory
{
    public Memory(IMemory cpu, IExtendedAddressing ea)
    {
        Byte = new Memory8Bit (cpu);
        Word = new Memory16Bit(cpu);
        DWord = new Memory32Bit (cpu);
        DP = new MemoryDP(cpu);
        Indexed = new MemoryIndexed (cpu, ea);
    }

    public Memory8Bit Byte { get; }
    public Memory16Bit Word { get; }
    public Memory32Bit DWord { get; }
    public MemoryDP DP { get; }
    public MemoryIndexed Indexed { get; }
}
