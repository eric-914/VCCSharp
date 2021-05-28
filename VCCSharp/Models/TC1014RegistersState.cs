namespace VCCSharp.Models
{
    public struct TC1014RegistersState
    {
        public byte EnhancedFIRQFlag;
        public byte EnhancedIRQFlag;
        public byte LastIrq;
        public byte LastFirq;

        public unsafe fixed byte GimeRegisters[256];
    }
}
