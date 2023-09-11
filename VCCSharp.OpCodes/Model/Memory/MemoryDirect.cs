using VCCSharp.OpCodes.Memory;

namespace VCCSharp.OpCodes.Model.Memory;

internal class MemoryDirect
{
    private readonly IAddressDP _cpu;

    public MemoryDirect(IAddressDP cpu)
    {
        _cpu = cpu;
    }

    public ushort this[ushort address]
    {
        get { return (ushort)((_cpu.DP << 8) | _cpu.MemRead8(address)); }
    }
}
