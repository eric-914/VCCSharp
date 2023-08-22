namespace VCCSharp.OpCodes.Memory;

public interface IMemory8bit
{
    byte MemRead8(ushort address);
    void MemWrite8(byte data, ushort address);
}
