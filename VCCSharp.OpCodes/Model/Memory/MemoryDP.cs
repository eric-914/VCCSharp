using VCCSharp.OpCodes.Memory;

namespace VCCSharp.OpCodes.Model.Memory;

internal class MemoryDP
{
    private readonly IAddressDP _cpu;

    public MemoryDP(IAddressDP cpu)
    {
        _cpu = cpu;
    }

    public ushort this[ushort address]
    {
        get { return _cpu.DPADDRESS(address); }
    }
}
