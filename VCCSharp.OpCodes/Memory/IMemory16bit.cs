namespace VCCSharp.OpCodes.Memory;

public interface IMemory16bit
{
    ushort MemRead16(ushort address);
    void MemWrite16(ushort data, ushort address);
}
