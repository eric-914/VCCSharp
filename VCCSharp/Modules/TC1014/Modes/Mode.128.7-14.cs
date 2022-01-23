namespace VCCSharp.Modules.TC1014.Modes
{
    // ReSharper disable once InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles
    public static class _128_7_14
    {
        //Bpp=0 Sr=7 1BPP Stretch=8 
        //Bpp=0 Sr=8
        //Bpp=0 Sr=9
        //Bpp=0 Sr=10
        //Bpp=0 Sr=11
        //Bpp=0 Sr=12
        //Bpp=0 Sr=13
        //Bpp=0 Sr=14
        public static void Mode(ModeModel model, int start, int yStride)
        {
            IGraphics graphics = model.Modules.Graphics;
            IEmu emu = model.Modules.Emu;

            uint[] palette = graphics.GetGraphicsColors().Palette32Bit;
            var szSurface32 = graphics.GetGraphicsSurface();
            int xPitch = (int)emu.SurfacePitch;
            var memory = model.ShortPointer;

            for (ushort beam = 0; beam < graphics.BytesPerRow; beam += 2) //1bbp Stretch=8
            {
                long index = (graphics.VidMask & (start + (byte)(graphics.HorizontalOffset + beam))) >> 1;
                ushort widePixel = memory[index];

                szSurface32[yStride += 1] = palette[1 & (widePixel >> 7)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 7)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 7)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 7)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 7)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 7)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 7)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 7)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 5)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 5)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 5)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 5)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 5)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 5)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 5)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 5)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 3)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 3)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 3)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 3)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 3)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 3)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 3)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 3)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 1)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 1)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 1)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 1)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 1)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 1)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 1)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 1)];
                szSurface32[yStride += 1] = palette[1 & widePixel];
                szSurface32[yStride += 1] = palette[1 & widePixel];
                szSurface32[yStride += 1] = palette[1 & widePixel];
                szSurface32[yStride += 1] = palette[1 & widePixel];
                szSurface32[yStride += 1] = palette[1 & widePixel];
                szSurface32[yStride += 1] = palette[1 & widePixel];
                szSurface32[yStride += 1] = palette[1 & widePixel];
                szSurface32[yStride += 1] = palette[1 & widePixel];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 15)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 15)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 15)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 15)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 15)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 15)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 15)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 15)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 13)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 13)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 13)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 13)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 13)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 13)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 13)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 13)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 11)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 11)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 11)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 11)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 11)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 11)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 11)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 11)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 9)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 9)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 9)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 9)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 9)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 9)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 9)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 9)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[1 & (widePixel >> 8)];

                if (!emu.ScanLines)
                {
                    yStride -= (128);
                    yStride += xPitch;
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 7)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 7)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 7)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 7)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 7)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 7)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 7)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 7)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 5)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 5)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 5)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 5)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 5)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 5)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 5)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 5)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 3)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 3)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 3)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 3)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 3)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 3)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 3)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 3)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 1)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 1)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 1)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 1)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 1)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 1)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 1)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 1)];
                    szSurface32[yStride += 1] = palette[1 & widePixel];
                    szSurface32[yStride += 1] = palette[1 & widePixel];
                    szSurface32[yStride += 1] = palette[1 & widePixel];
                    szSurface32[yStride += 1] = palette[1 & widePixel];
                    szSurface32[yStride += 1] = palette[1 & widePixel];
                    szSurface32[yStride += 1] = palette[1 & widePixel];
                    szSurface32[yStride += 1] = palette[1 & widePixel];
                    szSurface32[yStride += 1] = palette[1 & widePixel];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 15)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 15)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 15)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 15)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 15)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 15)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 15)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 15)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 13)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 13)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 13)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 13)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 13)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 13)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 13)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 13)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 11)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 11)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 11)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 11)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 11)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 11)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 11)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 11)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 9)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 9)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 9)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 9)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 9)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 9)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 9)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 9)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 8)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 8)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 8)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 8)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 8)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 8)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 8)];
                    szSurface32[yStride += 1] = palette[1 & (widePixel >> 8)];
                    yStride -= xPitch;
                }
            }
        }
    }
}
