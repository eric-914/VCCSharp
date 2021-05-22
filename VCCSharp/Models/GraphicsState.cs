namespace VCCSharp.Models
{
    public struct GraphicsState
    {
        public ushort PixelsperLine;
        public ushort TagY;
        public ushort VerticalOffsetRegister;
        public ushort VPitch;

        public uint DistoOffset;
        public uint NewStartofVidram;
        public uint StartOfVidRam;
        public uint VidMask;

        public byte BorderColor8;
        public ushort BorderColor16;
        public uint BorderColor32;

        public unsafe fixed byte Lpf[4];
        public unsafe fixed byte VcenterTable[4];
    }
}
