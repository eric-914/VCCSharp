namespace VCCSharp.Models.CPU.OpCodes
{
    public interface IBus
    {
        ushort DPADDRESS(ushort r);
        
        byte MemRead8(ushort address);
        void MemWrite8(byte data, ushort address);

        ushort MemRead16(ushort address);
    }
}
