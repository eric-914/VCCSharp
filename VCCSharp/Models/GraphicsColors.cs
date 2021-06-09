namespace VCCSharp.Models
{
    public class GraphicsColors
    {
        public uint[] Afacts32 = { 0, 0xFF8D1F, 0x0667FF, 0xFFFFFF, 0, 0x0667FF, 0xFF8D1F, 0xFFFFFF }; //[2][4];

        public byte[] ColorTable32Bit = { 0, 85, 170, 255 };

        public byte[] Palette = new byte[16];           //Coco 3 6 bit colors

        public uint[] Palette32Bit = new uint[16];      //Color values translated to 24/32 bits

        public uint[] PaletteLookup32 = new uint[128];      //[2][64];	//0 = RGB 1=comp 32BIT
    }
}
