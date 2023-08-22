using VCCSharp.OpCodes.Memory;

namespace VCCSharp.OpCodes.Model.Memory;

internal class Memory8Bit
{
    private readonly IMemory8bit _cpu;

    public Memory8Bit(IMemory8bit cpu)
    {
        _cpu = cpu;
    }

    public byte this[ushort address]
    {
        get { return _cpu.MemRead8(address); }
        set { _cpu.MemWrite8(value, address); }
    }
}
