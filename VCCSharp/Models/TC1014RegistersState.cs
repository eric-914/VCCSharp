namespace VCCSharp.Models
{
    public struct TC1014RegistersState
    {
        public byte EnhancedFIRQFlag;
        public byte EnhancedIRQFlag;
        public byte VDG_Mode;
        public byte Dis_Offset;
        public byte MPU_Rate;
        public byte LastIrq;
        public byte LastFirq;

        public ushort VerticalOffsetRegister;

        public short InterruptTimer;

        public unsafe fixed byte GimeRegisters[256];
        public unsafe byte* Rom;
    }
}
