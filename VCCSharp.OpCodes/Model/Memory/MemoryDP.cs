using VCCSharp.OpCodes.Memory;

namespace VCCSharp.OpCodes.Model.Memory;

internal class MemoryDP
{
    private readonly IAddressDP _cpu;

    public MemoryDP(IAddressDP cpu)
    {
        _cpu = cpu;
    }

    //TODO: Inline this to use regular memory accessors.  
    //SEE: ushort DPADDRESS(ushort r);
    public ushort this[ushort address]
    {
        get { return _cpu.DPADDRESS(address); }
    }
}
