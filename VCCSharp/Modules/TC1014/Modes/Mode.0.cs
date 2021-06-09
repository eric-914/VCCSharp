using VCCSharp.Modules.TC1014.Modes;

// ReSharper disable once CheckNamespace
namespace VCCSharp.Modules.TC1014
{
    //Width 80
    // ReSharper disable once InconsistentNaming
    public partial class TC1014
    {
        public unsafe void Mode0(ModeModel model, int start, int yStride)
        {
            uint[] textPalette = { 0, 0 };
            byte attributes = 0;

            uint[] palette = model.Modules.Graphics.GetGraphicsColors().Palette32Bit;
            uint* szSurface32 = model.Modules.Graphics.GetGraphicsSurfaces().pSurface32;
            byte* memory = model.Memory;
            ushort y = (ushort)(model.Modules.Emu.LineCounter);
            int xPitch = (int)model.Modules.Emu.SurfacePitch;

            if ((Graphics.HorizontalOffsetReg & 128) != 0)
            {
                start = (int)(Graphics.StartOfVidRam + (Graphics.TagY / Graphics.LinesPerRow) * (Graphics.VPitch)); //Fix for Horizontal Offset Register in text mode.
            }

            for (ushort beam = 0; beam < Graphics.BytesPerRow * Graphics.ExtendedText; beam += Graphics.ExtendedText)
            {
                byte character = memory[start + (byte)(beam + Graphics.HorizontalOffset)];
                byte pixel = CC3FontData8X12[character * 12 + (y % Graphics.LinesPerRow)];

                if (Graphics.ExtendedText == 2)
                {
                    attributes = memory[start + (byte)(beam + Graphics.HorizontalOffset) + 1];

                    if (((attributes & 64) != 0) && (y % Graphics.LinesPerRow == (Graphics.LinesPerRow - 1)))
                    {   //UnderLine
                        pixel = 255;
                    }

                    if (Graphics.CheckState(attributes))
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

                if (!_modules.Emu.ScanLines)
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
