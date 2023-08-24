using VCCSharp.OpCodes.Memory;

namespace VCCSharp.OpCodes.Model.Memory;

internal class MemoryIndexed
{
    private readonly IAddressIndexed _cpu;

    public MemoryIndexed(IAddressIndexed cpu)
    {
        _cpu = cpu;
    }

    //TODO: Inline this to use regular memory accessors.  This process takes cpu cycles in itself and needs to accommodate that fact.
    //SEE: ushort CalculateEA(byte postByte)
    public ushort this[ushort address]
    {
        get { return _cpu.INDADDRESS(address); }
    }
}
