namespace VCCSharp.Models
{
    public class GraphicsColors
    {
        //public byte[] Afacts8 = { 0, 0xA4, 0x89, 0xBF, 0, 137, 164, 191 }; //[2][4];
        //public ushort[] Afacts16 = { 0, 0xF800, 0x001F, 0xFFFF, 0, 0x001F, 0xF800, 0xFFFF }; //[2][4];
        public uint[] Afacts32 = { 0, 0xFF8D1F, 0x0667FF, 0xFFFFFF, 0, 0x0667FF, 0xFF8D1F, 0xFFFFFF }; //[2][4];

        //public byte[] ColorTable16Bit = { 0, 10, 21, 31 };	//Color brightness at 0 1 2 and 3 (2 bits)
        public byte[] ColorTable32Bit = { 0, 85, 170, 255 };

        public byte[] Palette = new byte[16];           //Coco 3 6 bit colors
        public byte[] Palette8Bit = new byte[16];
        public ushort[] Palette16Bit = new ushort[16];  //Color values translated to 16bit 32BIT
        public uint[] Palette32Bit = new uint[16];      //Color values translated to 24/32 bits

        public byte[] PaletteLookup8 = new byte[128];       //[2][64];	//0 = RGB 1=comp 8BIT
        public ushort[] PaletteLookup16 = new ushort[128];  //[2][64];	//0 = RGB 1=comp 16BIT
        public uint[] PaletteLookup32 = new uint[128];      //[2][64];	//0 = RGB 1=comp 32BIT
    }
}
