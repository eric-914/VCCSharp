using VCCSharp.Modules.TC1014.Modes;

// ReSharper disable once CheckNamespace
namespace VCCSharp.Modules.TC1014
{
    //******************************************************************** Low Res Text
    //Low Res text GraphicsMode=0 CompatMode=1 Ignore Bpp and Stretch
    // ReSharper disable once InconsistentNaming
    public partial class TC1014
    {
        public unsafe void Mode64_127(ModeModel model, int start, int yStride)
        {
            uint[] textPalette = { 0, 0 };
            byte pixel = 0;

            uint[] palette = model.Modules.Graphics.GetGraphicsColors().Palette32Bit;
            uint* szSurface32 = model.Modules.Graphics.GetGraphicsSurfaces().pSurface32;
            ushort y = (ushort)(model.Modules.Emu.LineCounter);
            int xPitch = (int)model.Modules.Emu.SurfacePitch;

            for (ushort beam = 0; beam < Graphics.BytesPerRow; beam++)
            {
                byte character = model.Memory[start + (byte)(beam + Graphics.HorizontalOffset)];

                switch ((character & 192) >> 6)
                {
                    case 0:
                        character &= 63;
                        textPalette[0] = palette[Graphics.TextBgPalette];
                        textPalette[1] = palette[Graphics.TextFgPalette];

                        if ((Graphics.LowerCase != 0) && (character < 32))
                            pixel = NTSCRoundFontData8x12[(character + 80) * 12 + (y % 12)];
                        else
                            pixel = (byte)~NTSCRoundFontData8x12[(character) * 12 + (y % 12)];
                        break;

                    case 1:
                        character &= 63;
                        textPalette[0] = palette[Graphics.TextBgPalette];
                        textPalette[1] = palette[Graphics.TextFgPalette];
                        pixel = NTSCRoundFontData8x12[(character) * 12 + (y % 12)];
                        break;

                    case 2:
                    case 3:
                        textPalette[1] = palette[(character & 112) >> 4];
                        textPalette[0] = palette[8];
                        character = (byte)(64 + (character & 0xF));
                        pixel = NTSCRoundFontData8x12[(character) * 12 + (y % 12)];
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

                if (!_modules.Emu.ScanLines)
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
