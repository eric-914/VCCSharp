namespace VCCSharp.Modules.TC1014.Modes
{
    // ReSharper disable once InconsistentNaming
    public static class _128_1_2
    {
        //Bpp=0 Sr=1 1BPP Stretch=2
        //Bpp=0 Sr=2 
        public static unsafe void Mode(ModeModel model, int start, int yStride)
        {
            IGraphics graphics = model.Modules.Graphics;
            IEmu emu = model.Modules.Emu;

            uint[] palette = graphics.GetGraphicsColors().Palette32Bit;
            uint* szSurface32 = graphics.GetGraphicsSurfaces().pSurface32;
            ushort* wMemory = (ushort*) model.Memory;
            int xPitch = (int)emu.SurfacePitch;

            for (ushort beam = 0; beam < graphics.BytesPerRow; beam += 2) //1bbp Stretch=2
            {
                ushort widePixel = wMemory[(graphics.VidMask & (start + (byte)(graphics.HorizontalOffset + beam))) >> 1];

                szSurface32[yStride += 1] = palette[1 & (widePixel >> 7)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 7)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 5)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 5)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 3)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 3)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 1)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 1)];
                szSurface32[yStride += 1] = palette[1 & widePixel];
                szSurface32[yStride += 1] = palette[1 & widePixel];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 15)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 15)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 13)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 13)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 11)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 11)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 9)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 9)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 8)];

                if (!emu.ScanLines)
                {
                    yStride -= (32);
                    yStride += xPitch;
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 7)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 7)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 5)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 5)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 3)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 3)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 1)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 1)];
                    szSurface32[yStride += 1] = palette[1 & widePixel];
                    szSurface32[yStride += 1] = palette[1 & widePixel];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 15)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 15)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 13)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 13)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 11)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 11)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 9)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 9)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 8)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 8)];
                    yStride -= xPitch;
                }
            }
        }
    }
}
