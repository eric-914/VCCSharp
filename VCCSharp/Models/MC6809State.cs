namespace VCCSharp.Models
{
    public struct MC6809State
    {
        public MC6809CpuRegister pc, x, y, u, s, dp, d;
        
        public byte ccbits;
        public unsafe fixed uint cc[8];

        public unsafe fixed long /* byte* */ ureg8[8];
        public unsafe fixed long /* ushort* */ xfreg16[8];
    }
}
