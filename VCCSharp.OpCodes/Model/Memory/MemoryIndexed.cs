using VCCSharp.OpCodes.Memory;

namespace VCCSharp.OpCodes.Model.Memory;

internal class MemoryIndexed
{
    private readonly IAddressIndexed _cpu;

    public MemoryIndexed(IAddressIndexed cpu)
    {
        _cpu = cpu;
    }

    public ushort this[ushort address]
    {
        get { return _cpu.INDADDRESS(address); }
    }
}
