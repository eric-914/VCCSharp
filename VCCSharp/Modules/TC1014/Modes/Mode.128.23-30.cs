﻿namespace VCCSharp.Modules.TC1014.Modes
{
    // ReSharper disable once InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles
    public static class _128_23_30
    {
        //Bpp=1 Sr=7  2BPP Stretch=8
        //Bpp=1 Sr=8
        //Bpp=1 Sr=9 
        //Bpp=1 Sr=10 
        //Bpp=1 Sr=11 
        //Bpp=1 Sr=12 
        //Bpp=1 Sr=13 
        //Bpp=1 Sr=14
        public static unsafe void Mode(ModeModel model, int start, int yStride)
        {
            IGraphics graphics = model.Modules.Graphics;
            IEmu emu = model.Modules.Emu;

            uint[] palette = graphics.GetGraphicsColors().Palette32Bit;
            uint* szSurface32 = graphics.GetGraphicsSurface();
            ushort* wMemory = (ushort*) model.Memory;
            int xPitch = (int)emu.SurfacePitch;

            for (ushort beam = 0; beam < graphics.BytesPerRow; beam += 2) //2bbp Stretch=8
            {
                uint widePixel = wMemory[(graphics.VidMask & (start + (byte)(graphics.HorizontalOffset + beam))) >> 1];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[3 & widePixel];
                szSurface32[yStride += 1] = palette[3 & widePixel];
                szSurface32[yStride += 1] = palette[3 & widePixel];
                szSurface32[yStride += 1] = palette[3 & widePixel];
                szSurface32[yStride += 1] = palette[3 & widePixel];
                szSurface32[yStride += 1] = palette[3 & widePixel];
                szSurface32[yStride += 1] = palette[3 & widePixel];
                szSurface32[yStride += 1] = palette[3 & widePixel];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];

                if (!emu.ScanLines)
                {
                    yStride -= (64);
                    yStride += xPitch;
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[3 & widePixel];
                    szSurface32[yStride += 1] = palette[3 & widePixel];
                    szSurface32[yStride += 1] = palette[3 & widePixel];
                    szSurface32[yStride += 1] = palette[3 & widePixel];
                    szSurface32[yStride += 1] = palette[3 & widePixel];
                    szSurface32[yStride += 1] = palette[3 & widePixel];
                    szSurface32[yStride += 1] = palette[3 & widePixel];
                    szSurface32[yStride += 1] = palette[3 & widePixel];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                    yStride -= xPitch;
                }
            }
        }
    }
}
