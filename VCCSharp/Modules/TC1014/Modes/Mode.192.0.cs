﻿using VCCSharp.Modules.TC1014.Modes;

// ReSharper disable once CheckNamespace
namespace VCCSharp.Modules.TC1014
{
    // ReSharper disable once InconsistentNaming
    public partial class TC1014
    {
        //Bpp=0 Sr=0 1BPP Stretch=1
        public unsafe void Mode192_0(ModeModel model, int start, int yStride)
        {
            uint[] palette = model.Modules.Graphics.GetGraphicsColors().Palette32Bit;
            uint* szSurface32 = model.Modules.Graphics.GetGraphicsSurfaces().pSurface32;
            ushort* wMemory = (ushort*) model.Memory;
            int xPitch = (int)model.Modules.Emu.SurfacePitch;

            for (ushort beam = 0; beam < Graphics.BytesPerRow; beam += 2) //1bbp Stretch=1
            {
                uint widePixel = wMemory[(Graphics.VidMask & (start + (byte)(Graphics.HorizontalOffset + beam))) >> 1];
                szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & widePixel)];
                szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 8))];

                if (!_modules.Emu.ScanLines)
                {
                    yStride -= (16);
                    yStride += xPitch;
                    szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                    szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                    szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                    szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                    szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                    szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                    szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                    szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & widePixel)];
                    szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                    szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                    szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                    szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                    szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                    szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                    szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                    szSurface32[yStride += 1] = palette[Graphics.PaletteIndex + (1 & (widePixel >> 8))];
                    yStride -= xPitch;
                }
            }
        }
    }
}
