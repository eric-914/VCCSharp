namespace VCCSharp.Models
{
    public struct HD6309State
    {
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

        public unsafe fixed byte InsCycles[2 * 25]; //[2][25];

        public unsafe fixed long /* byte* */ NatEmuCycles[24];
    }
}
