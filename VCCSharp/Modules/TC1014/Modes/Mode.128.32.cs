﻿namespace VCCSharp.Modules.TC1014.Modes
{
    // ReSharper disable once InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles
    public static class _128_32
    {
        //Bpp=2 Sr=0 4BPP Stretch=1
        public static void Mode(ModeModel model, int start, int yStride)
        {
            IGraphics graphics = model.Modules.Graphics;
            IEmu emu = model.Modules.Emu;

            uint[] palette = graphics.GetGraphicsColors().Palette32Bit;
            var szSurface32 = graphics.GetGraphicsSurface();
            int xPitch = (int)emu.SurfacePitch;
            var memory = model.ShortPointer;

            for (ushort beam = 0; beam < graphics.BytesPerRow; beam += 2) //4bbp Stretch=1
            {
                long index = (graphics.VidMask & (start + (byte)(graphics.HorizontalOffset + beam))) >> 1;
                ushort widePixel = memory[index];

                szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[15 & widePixel];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];

                if (!emu.ScanLines)
                {
                    yStride -= (4);
                    yStride += xPitch;
                    szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[15 & widePixel];
                    szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
                    yStride -= xPitch;
                }
            }
        }
    }
}
