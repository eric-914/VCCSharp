using VCCSharp.OpCodes.Memory;
using VCCSharp.OpCodes.Model.OpCodes;
using VCCSharp.OpCodes.Model.Support;

namespace VCCSharp.OpCodes.Model.Memory;

internal class MemoryIndexed : ITempAccess
{
    private readonly IMemory8bit _cpu;
    private readonly IExtendedAddressing _ea;

    public MemoryIndexed(IMemory8bit cpu, IExtendedAddress ea)
    {
        _cpu = cpu;

        //TODO: The current EA class only handles 6809 rules.
        _ea = new ExtendedAddressing(ea);
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

    public IExtendedAddressing EA => _ea;
}
