using VCCSharp.OpCodes.Memory;

namespace VCCSharp.OpCodes.Model.Memory;

internal class Memory16Bit
{
    public readonly IMemory16bit _cpu;

    public Memory16Bit(IMemory16bit cpu)
    {
        _cpu = cpu;
    }

    public ushort this[ushort address]
    {
        get { return _cpu.MemRead16(address); }
        set { _cpu.MemWrite16(value, address); }
    }
}
