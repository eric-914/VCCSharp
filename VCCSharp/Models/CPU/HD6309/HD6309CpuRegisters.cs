namespace VCCSharp.Models.CPU.HD6309
{
    public struct HD6309CpuRegisters
    {
        public HD6309CpuRegister pc, x, y, u, s, dp, v, z;
        public HD6309WideRegister q;

        public byte ccbits;
        public byte mdbits;

        public unsafe fixed byte cc[8];
        public unsafe fixed uint md[8];

        public unsafe fixed long /* byte* */ ureg8[8];
        public unsafe fixed long /* ushort* */ xfreg16[8];
    }
}
