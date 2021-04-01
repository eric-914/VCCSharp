namespace VCCSharp.Models
{
    public struct HD6309State
    {
        public HD6309CpuRegister pc, x, y, u, s, dp, v, z;
        public HD6309WideRegister q;

        public unsafe fixed byte cc[8];
        public byte ccbits;
        public unsafe fixed uint md[8];
        public byte mdbits;

        public unsafe fixed long /* byte* */ ureg8[8];
        public unsafe fixed long /* ushort* */ xfreg16[8];

        public byte InInterrupt;
        public int CycleCounter;
        public uint SyncWaiting;
        public int gCycleFor;

        public byte NatEmuCycles65;
        public byte NatEmuCycles64;
        public byte NatEmuCycles32;
        public byte NatEmuCycles21;
        public byte NatEmuCycles54;
        public byte NatEmuCycles97;
        public byte NatEmuCycles85;
        public byte NatEmuCycles51;
        public byte NatEmuCycles31;
        public byte NatEmuCycles1110;
        public byte NatEmuCycles76;
        public byte NatEmuCycles75;
        public byte NatEmuCycles43;
        public byte NatEmuCycles87;
        public byte NatEmuCycles86;
        public byte NatEmuCycles98;
        public byte NatEmuCycles2726;
        public byte NatEmuCycles3635;
        public byte NatEmuCycles3029;
        public byte NatEmuCycles2827;
        public byte NatEmuCycles3726;
        public byte NatEmuCycles3130;
        public byte NatEmuCycles42;
        public byte NatEmuCycles53;

        //--Interrupt states
        public byte IRQWaiter;
        public byte PendingInterrupts;
        public unsafe fixed byte InsCycles[2 * 25]; //[2][25];

        public unsafe fixed long /* byte* */ NatEmuCycles[24];
    }
}
