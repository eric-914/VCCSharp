namespace VCCSharp.Models.Graphics
{
    public class GraphicsColors
    {
        public uint[] Artifacts32 { get; } = { 0, 0xFF8D1F, 0x0667FF, 0xFFFFFF, 0, 0x0667FF, 0xFF8D1F, 0xFFFFFF }; //[2][4];

        public byte[] ColorTable32Bit { get; } = { 0, 85, 170, 255 };

        public byte[] Palette { get; } = new byte[16];  //Coco 3 6 bit colors

        public Palette32 Palette32Bit { get; } = new(); //Color values translated to 24/32 bits

        public PaletteLookup32 PaletteLookup32 { get; } = new();
    }
}
