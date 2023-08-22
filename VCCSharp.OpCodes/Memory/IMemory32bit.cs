namespace VCCSharp.OpCodes.Memory;

public interface IMemory32bit
{
    uint MemRead32(ushort address);
    void MemWrite32(uint data, ushort address);
}
