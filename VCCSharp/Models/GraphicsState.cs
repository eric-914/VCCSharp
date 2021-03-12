namespace VCCSharp.Models
{
    public struct GraphicsState
    {
        public byte BlinkState;
        public byte BorderChange;
        public byte Bpp;
        public byte BytesperRow;
        public byte CC2Offset;
        public byte CC2VDGMode;
        public byte CC2VDGPiaMode;
        public byte CC3BorderColor;
        public byte CC3Vmode;
        public byte CC3Vres;
        public byte ColorInvert;
        public byte CompatMode;
        public byte ExtendedText;
        public byte GraphicsMode;
        public byte Hoffset;
        public byte HorzCenter;
        public byte HorzOffsetReg;
        public byte InvertAll;
        public byte LinesperRow;
        public byte LinesperScreen;
        public byte LowerCase;
        public byte MasterMode;
        public byte MonType;
        public byte PaletteIndex;
        public byte Stretch;
        public byte TextBGColor;
        public byte TextBGPalette;
        public byte TextFGColor;
        public byte TextFGPalette;
        public byte VertCenter;
        public byte VresIndex;

        public ushort PixelsperLine;
        public ushort TagY;
        public ushort VerticalOffsetRegister;
        public ushort VPitch;

        public uint DistoOffset;
        public uint NewStartofVidram;
        public uint StartofVidram;
        public uint VidMask;

        public byte BorderColor8;
        public ushort BorderColor16;
        public uint BorderColor32;

        public unsafe fixed byte Lpf[4];
        public unsafe fixed byte VcenterTable[4];
    }
}
