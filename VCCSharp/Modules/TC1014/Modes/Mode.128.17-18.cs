namespace VCCSharp.Modules.TC1014.Modes
{
    // ReSharper disable once InconsistentNaming
    public static class _128_17_18
    {
        //Bpp=0 Sr=15 1BPP Stretch=16
        //BPP=1 Sr=0  2BPP Stretch=1
        public static unsafe void Mode(ModeModel model, int start, int yStride)
        {
            IGraphics graphics = model.Modules.Graphics;
            IEmu emu = model.Modules.Emu;

            uint[] palette = graphics.GetGraphicsColors().Palette32Bit;
            uint* szSurface32 = graphics.GetGraphicsSurfaces().pSurface32;
            ushort* wMemory = (ushort*) model.Memory;
            int xPitch = (int)emu.SurfacePitch;

            for (ushort beam = 0; beam < graphics.BytesPerRow; beam += 2) //2bbp Stretch=2
            {
                uint widePixel = wMemory[(graphics.VidMask & (start + (byte)(graphics.HorizontalOffset + beam))) >> 1];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[3 & widePixel];
                szSurface32[yStride += 1] = palette[3 & widePixel];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];

                if (!emu.ScanLines)
                {
                    yStride -= (16);
                    yStride += xPitch;
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[3 & widePixel];
                    szSurface32[yStride += 1] = palette[3 & widePixel];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                    yStride -= xPitch;
                }
            }
        }
    }
}
