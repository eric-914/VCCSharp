using VCCSharp.OpCodes.Memory;

namespace VCCSharp.OpCodes.Model.Memory;

internal class Memory32Bit
{
    private readonly IMemory32bit _cpu;

    public Memory32Bit(IMemory32bit cpu)
    {
        _cpu = cpu;
    }

    public uint this[ushort address]
    {
        get { return _cpu.MemRead32(address); }
        set { _cpu.MemWrite32(value, address); }
    }
}
