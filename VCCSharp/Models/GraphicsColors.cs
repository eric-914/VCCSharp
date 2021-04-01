namespace VCCSharp.Models
{
    public struct GraphicsColors
    {
        public unsafe fixed byte Afacts8[8]; //[2][4];
        public unsafe fixed ushort Afacts16[8]; //[2][4];
        public unsafe fixed uint Afacts32[8]; //[2][4];

        public unsafe fixed byte ColorTable16Bit[4];
        public unsafe fixed byte ColorTable32Bit[4];

        public unsafe fixed byte Palette[16];       //Coco 3 6 bit colors
        public unsafe fixed byte Palette8Bit[16];
        public unsafe fixed ushort Palette16Bit[16];  //Color values translated to 16bit 32BIT
        public unsafe fixed uint Palette32Bit[16];  //Color values translated to 24/32 bits

        public unsafe fixed byte PaletteLookup8[128]; //[2][64];	  //0 = RGB 1=comp 8BIT
        public unsafe fixed ushort PaletteLookup16[128]; //[2][64];	//0 = RGB 1=comp 16BIT
        public unsafe fixed uint PaletteLookup32[128]; //[2][64];	//0 = RGB 1=comp 32BIT
    }
}
