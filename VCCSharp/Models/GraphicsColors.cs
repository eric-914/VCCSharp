using VCCSharp.Enums;

namespace VCCSharp.Models
{
    public class GraphicsColors
    {
        public uint[] Artifacts32 { get; } = { 0, 0xFF8D1F, 0x0667FF, 0xFFFFFF, 0, 0x0667FF, 0xFF8D1F, 0xFFFFFF }; //[2][4];

        public byte[] ColorTable32Bit { get; } = { 0, 85, 170, 255 };

        public byte[] Palette { get; } = new byte[16];           //Coco 3 6 bit colors

        public uint[] Palette32Bit { get; } = new uint[16];      //Color values translated to 24/32 bits

        public PaletteLookup32 PaletteLookup32 { get; } = new PaletteLookup32();  //[2][64];	//0 = RGB 1=comp 32BIT
    }

    public class PaletteLookup32
    {
        private readonly UintArray _composite = new UintArray();
        private readonly UintArray _rgb = new UintArray();

        public UintArray this[MonitorTypes monitorType]
        {
            get
            {
                switch (monitorType)
                {
                    case MonitorTypes.Composite: return _composite;
                    case MonitorTypes.RGB: return _rgb;
                }

                return null;
            }
        }
    }

    public class UintArray
    {
        private readonly uint[] _array = new uint[64];

        public uint this[int index]
        {
            get => _array[index];
            set => _array[index] = value;
        }
    }
}
