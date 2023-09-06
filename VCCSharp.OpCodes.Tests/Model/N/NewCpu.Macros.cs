using VCCSharp.OpCodes.Definitions;
using VCCSharp.OpCodes.MC6809;

namespace VCCSharp.OpCodes.Tests;

internal partial class NewCpu : IState
{
    public Mode Mode => Mode.MC6809;

    public ushort DPADDRESS(ushort r) => (ushort)((DP << 8) | MemRead8(r)); //DIRECT PAGE REGISTER

    public ushort MemRead16(ushort address)
    {
        return (ushort)(MemRead8(address) << 8 | MemRead8((ushort)(address + 1)));
    }

    public byte MemRead8(ushort address)
    {
        return _mem.Read(address);
    }

    public void MemWrite16(ushort data, ushort address)
    {
        MemWrite8((byte)(data >> 8), address);
        MemWrite8((byte)(data & 0xFF), (ushort)(address + 1));
    }

    public void MemWrite8(byte data, ushort address)
    {
        _mem.Write(address, data);
    }
}
