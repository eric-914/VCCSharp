using VCCSharp.Modules.TC1014.Modes;

// ReSharper disable once CheckNamespace
namespace VCCSharp.Modules.TC1014
{
    // ReSharper disable once InconsistentNaming
    public partial class TC1014
    {
        //Bpp=0 Sr=15 1BPP Stretch=16
        //BPP=1 Sr=0  2BPP Stretch=1
        public unsafe void Mode192_15_16(ModeModel model, int start, int yStride)
        {
            uint[] palette = model.Modules.Graphics.GetGraphicsColors().Palette32Bit;
            uint* szSurface32 = model.Modules.Graphics.GetGraphicsSurfaces().pSurface32;
            ushort* wMemory = (ushort*) model.Memory;
            int xPitch = (int)model.Modules.Emu.SurfacePitch;

            for (ushort beam = 0; beam < Graphics.BytesPerRow; beam += 2) //2bbp Stretch=1
            {
                uint widePixel = wMemory[(Graphics.VidMask & (start + (byte)(Graphics.HorizontalOffset + beam))) >> 1];
                szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (3 & widePixel)];
                szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (3 & (widePixel >> 8))];

                if (!_modules.Emu.ScanLines)
                {
                    yStride -= (8);
                    yStride += xPitch;
                    szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                    szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                    szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                    szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (3 & widePixel)];
                    szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                    szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                    szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                    szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                    yStride -= xPitch;
                }
            }
        }
    }
}
