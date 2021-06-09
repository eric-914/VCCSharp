namespace VCCSharp.Modules.TC1014.Modes
{
    //******************************************************************** Low Res Text
    //Low Res text GraphicsMode=0 CompatMode=1 Ignore Bpp and Stretch
    // ReSharper disable once InconsistentNaming
    public static class _64_127
    {
        public static unsafe void Mode(ModeModel model, int start, int yStride)
        {
            uint[] textPalette = { 0, 0 };
            byte pixel = 0;

            IGraphics graphics = model.Modules.Graphics;
            IEmu emu = model.Modules.Emu;

            uint[] palette = graphics.GetGraphicsColors().Palette32Bit;
            uint* szSurface32 = graphics.GetGraphicsSurfaces().pSurface32;
            ushort y = (ushort)(emu.LineCounter);
            int xPitch = (int)emu.SurfacePitch;

            for (ushort beam = 0; beam < graphics.BytesPerRow; beam++)
            {
                byte character = model.Memory[start + (byte)(beam + graphics.HorizontalOffset)];

                switch ((character & 192) >> 6)
                {
                    case 0:
                        character &= 63;
                        textPalette[0] = palette[graphics.TextBgPalette];
                        textPalette[1] = palette[graphics.TextFgPalette];

                        if ((graphics.LowerCase != 0) && (character < 32))
                            pixel = TC1014.NTSCRoundFontData8x12[(character + 80) * 12 + (y % 12)];
                        else
                            pixel = (byte)~TC1014.NTSCRoundFontData8x12[(character) * 12 + (y % 12)];
                        break;

                    case 1:
                        character &= 63;
                        textPalette[0] = palette[graphics.TextBgPalette];
                        textPalette[1] = palette[graphics.TextFgPalette];
                        pixel = TC1014.NTSCRoundFontData8x12[(character) * 12 + (y % 12)];
                        break;

                    case 2:
                    case 3:
                        textPalette[1] = palette[(character & 112) >> 4];
                        textPalette[0] = palette[8];
                        character = (byte)(64 + (character & 0xF));
                        pixel = TC1014.NTSCRoundFontData8x12[(character) * 12 + (y % 12)];
                        break;
                }

                szSurface32[yStride += 1] = textPalette[(pixel >> 7)];
                szSurface32[yStride += 1] = textPalette[(pixel >> 7)];
                szSurface32[yStride += 1] = textPalette[(pixel >> 6) & 1];
                szSurface32[yStride += 1] = textPalette[(pixel >> 6) & 1];
                szSurface32[yStride += 1] = textPalette[(pixel >> 5) & 1];
                szSurface32[yStride += 1] = textPalette[(pixel >> 5) & 1];
                szSurface32[yStride += 1] = textPalette[(pixel >> 4) & 1];
                szSurface32[yStride += 1] = textPalette[(pixel >> 4) & 1];
                szSurface32[yStride += 1] = textPalette[(pixel >> 3) & 1];
                szSurface32[yStride += 1] = textPalette[(pixel >> 3) & 1];
                szSurface32[yStride += 1] = textPalette[(pixel >> 2) & 1];
                szSurface32[yStride += 1] = textPalette[(pixel >> 2) & 1];
                szSurface32[yStride += 1] = textPalette[(pixel >> 1) & 1];
                szSurface32[yStride += 1] = textPalette[(pixel >> 1) & 1];
                szSurface32[yStride += 1] = textPalette[(pixel & 1)];
                szSurface32[yStride += 1] = textPalette[(pixel & 1)];

                if (!emu.ScanLines)
                {
                    yStride -= (16);
                    yStride += xPitch;
                    szSurface32[yStride += 1] = textPalette[(pixel >> 7)];
                    szSurface32[yStride += 1] = textPalette[(pixel >> 7)];
                    szSurface32[yStride += 1] = textPalette[(pixel >> 6) & 1];
                    szSurface32[yStride += 1] = textPalette[(pixel >> 6) & 1];
                    szSurface32[yStride += 1] = textPalette[(pixel >> 5) & 1];
                    szSurface32[yStride += 1] = textPalette[(pixel >> 5) & 1];
                    szSurface32[yStride += 1] = textPalette[(pixel >> 4) & 1];
                    szSurface32[yStride += 1] = textPalette[(pixel >> 4) & 1];
                    szSurface32[yStride += 1] = textPalette[(pixel >> 3) & 1];
                    szSurface32[yStride += 1] = textPalette[(pixel >> 3) & 1];
                    szSurface32[yStride += 1] = textPalette[(pixel >> 2) & 1];
                    szSurface32[yStride += 1] = textPalette[(pixel >> 2) & 1];
                    szSurface32[yStride += 1] = textPalette[(pixel >> 1) & 1];
                    szSurface32[yStride += 1] = textPalette[(pixel >> 1) & 1];
                    szSurface32[yStride += 1] = textPalette[(pixel & 1)];
                    szSurface32[yStride += 1] = textPalette[(pixel & 1)];
                    yStride -= xPitch;
                }
            }
        }
    }
}
