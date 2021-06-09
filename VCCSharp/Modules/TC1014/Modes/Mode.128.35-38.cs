namespace VCCSharp.Modules.TC1014.Modes
{
    // ReSharper disable once InconsistentNaming
    public static class _128_35_38
    {
        //Bpp=2 Sr=3 4BPP Stretch=4
        //Bpp=2 Sr=4 
        //Bpp=2 Sr=5 
        //Bpp=2 Sr=6 
        public static unsafe void Mode(ModeModel model, int start, int yStride)
        {
            IGraphics graphics = model.Modules.Graphics;
            IEmu emu = model.Modules.Emu;

            uint[] palette = graphics.GetGraphicsColors().Palette32Bit;
            uint* szSurface32 = graphics.GetGraphicsSurfaces().pSurface32;
            ushort* wMemory = (ushort*) model.Memory;
            int xPitch = (int)emu.SurfacePitch;

            for (ushort beam = 0; beam < graphics.BytesPerRow; beam += 2) //4bbp Stretch=4
            {
                uint widePixel = wMemory[(graphics.VidMask & (start + (byte)(graphics.HorizontalOffset + beam))) >> 1];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[15 & widePixel];
                szSurface32[yStride += 1] = palette[15 & widePixel];
                szSurface32[yStride += 1] = palette[15 & widePixel];
                szSurface32[yStride += 1] = palette[15 & widePixel];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];

                if (!emu.ScanLines)
                {
                    yStride -= (16);
                    yStride += xPitch;
                    szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[15 & widePixel];
                    szSurface32[yStride += 1] = palette[15 & widePixel];
                    szSurface32[yStride += 1] = palette[15 & widePixel];
                    szSurface32[yStride += 1] = palette[15 & widePixel];
                    szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
                    szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
                    szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
                    szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
                    yStride -= xPitch;
                }
            }
        }
    }
}
