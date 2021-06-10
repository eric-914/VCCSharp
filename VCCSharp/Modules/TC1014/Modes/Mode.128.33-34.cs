namespace VCCSharp.Modules.TC1014.Modes
{
    // ReSharper disable once InconsistentNaming
    public static class _128_33_34
    {
        public static unsafe void Mode(ModeModel model, int start, int yStride)
        {
            IGraphics graphics = model.Modules.Graphics;
            IEmu emu = model.Modules.Emu;

            uint[] palette = graphics.GetGraphicsColors().Palette32Bit;
            uint* szSurface32 = graphics.GetGraphicsSurface();
            ushort* wMemory = (ushort*) model.Memory;
            int xPitch = (int)emu.SurfacePitch;

            for (ushort beam = 0; beam < graphics.BytesPerRow; beam += 2) //4bbp Stretch=2
            {
                uint widePixel = wMemory[(graphics.VidMask & (start + (byte)(graphics.HorizontalOffset + beam))) >> 1];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[15 & widePixel];
                szSurface32[yStride += 1] = palette[15 & widePixel];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];

                if (!emu.ScanLines)
                {
                    yStride -= (8);
                    yStride += xPitch;
                    szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[15 & widePixel];
                    szSurface32[yStride += 1] = palette[15 & widePixel];
                    szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
                    szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
                    yStride -= xPitch;
                }
            }
        }
    }
}
