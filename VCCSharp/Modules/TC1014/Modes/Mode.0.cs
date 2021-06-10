namespace VCCSharp.Modules.TC1014.Modes
{
    //Width 80
    // ReSharper disable once InconsistentNaming
    public static class _0
    {
        public static unsafe void Mode(ModeModel model, int start, int yStride)
        {
            uint[] textPalette = { 0, 0 };
            byte attributes = 0;

            IGraphics graphics = model.Modules.Graphics;
            IEmu emu = model.Modules.Emu;

            uint[] palette = graphics.GetGraphicsColors().Palette32Bit;
            uint* szSurface32 = graphics.GetGraphicsSurface();
            byte* memory = model.Memory;
            ushort y = (ushort)(emu.LineCounter);
            int xPitch = (int)emu.SurfacePitch;

            if ((graphics.HorizontalOffsetReg & 128) != 0)
            {
                start = (int)(graphics.StartOfVidRam + (graphics.TagY / graphics.LinesPerRow) * (graphics.VPitch)); //Fix for Horizontal Offset Register in text mode.
            }

            for (ushort beam = 0; beam < graphics.BytesPerRow * graphics.ExtendedText; beam += graphics.ExtendedText)
            {
                byte character = memory[start + (byte)(beam + graphics.HorizontalOffset)];
                byte pixel = Fonts.CC3FontData8X12[character * 12 + (y % graphics.LinesPerRow)];

                if (graphics.ExtendedText == 2)
                {
                    attributes = memory[start + (byte)(beam + graphics.HorizontalOffset) + 1];

                    if (((attributes & 64) != 0) && (y % graphics.LinesPerRow == (graphics.LinesPerRow - 1)))
                    {   //UnderLine
                        pixel = 255;
                    }

                    if (graphics.CheckState(attributes))
                    {
                        pixel = 0;
                    }
                }

                textPalette[1] = palette[8 + ((attributes & 56) >> 3)];
                textPalette[0] = palette[attributes & 7];
                szSurface32[yStride += 1] = textPalette[pixel >> 7];
                szSurface32[yStride += 1] = textPalette[(pixel >> 6) & 1];
                szSurface32[yStride += 1] = textPalette[(pixel >> 5) & 1];
                szSurface32[yStride += 1] = textPalette[(pixel >> 4) & 1];
                szSurface32[yStride += 1] = textPalette[(pixel >> 3) & 1];
                szSurface32[yStride += 1] = textPalette[(pixel >> 2) & 1];
                szSurface32[yStride += 1] = textPalette[(pixel >> 1) & 1];
                szSurface32[yStride += 1] = textPalette[pixel & 1];

                if (!emu.ScanLines)
                {
                    yStride -= (8);
                    yStride += xPitch;
                    szSurface32[yStride += 1] = textPalette[pixel >> 7];
                    szSurface32[yStride += 1] = textPalette[(pixel >> 6) & 1];
                    szSurface32[yStride += 1] = textPalette[(pixel >> 5) & 1];
                    szSurface32[yStride += 1] = textPalette[(pixel >> 4) & 1];
                    szSurface32[yStride += 1] = textPalette[(pixel >> 3) & 1];
                    szSurface32[yStride += 1] = textPalette[(pixel >> 2) & 1];
                    szSurface32[yStride += 1] = textPalette[(pixel >> 1) & 1];
                    szSurface32[yStride += 1] = textPalette[pixel & 1];
                    yStride -= xPitch;
                }
            }
        }
    }
}
