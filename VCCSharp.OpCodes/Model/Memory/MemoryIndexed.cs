using VCCSharp.OpCodes.Memory;

namespace VCCSharp.OpCodes.Model.Memory;

internal class MemoryIndexed
{
    private readonly IMemory8bit _cpu;
    private readonly IExtendedAddressing _ea;

    public MemoryIndexed(IMemory8bit cpu, IExtendedAddressing ea)
    {
        _cpu = cpu;
        _ea = ea;
    }

    //TODO: This process takes cpu cycles in itself and needs to accommodate that fact.
    public ushort this[ushort address]
    {
        get
        {
            byte value = _cpu.MemRead8(address);
            return _ea.CalculateEA(value);
        }
    }
}
