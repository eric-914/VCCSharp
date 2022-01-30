// ReSharper disable once InconsistentNaming

using System.Diagnostics;
using VCCSharp.IoC;

#pragma warning disable IDE1006 // Naming Styles
namespace VCCSharp.Modules.TC1014.Modes
{
    //********************************************************************
    // Low Res Text
    // Low Res text GraphicsMode=0 CompatibilityMode=1 Ignore Bpp and Stretch
    //********************************************************************
    public static class _64_127
    {
        public static void Mode(ModeModel model, int start, int yStride)
        {
            IGraphics graphics = model.Modules.Graphics;
            IEmu emu = model.Modules.Emu;

            var szSurface32 = graphics.GetGraphicsSurface();
            uint[] textPalette = { 0, 0 };
            byte pixel = 0;

            uint[] palette = graphics.GetGraphicsColors().Palette32Bit;
            ushort y = (ushort)emu.LineCounter;
            int xPitch = (int)emu.SurfacePitch;
            BytePointer memory = model.BytePointer;

            void Render()
            {
                //--Render each bit twice apparently.
                void RenderBit(int bit)
                {
                    szSurface32[yStride += 1] = textPalette[bit];
                    szSurface32[yStride += 1] = textPalette[bit];
                }

                RenderBit(pixel >> 7);
                RenderBit((pixel >> 6) & 1);
                RenderBit((pixel >> 5) & 1);
                RenderBit((pixel >> 4) & 1);
                RenderBit((pixel >> 3) & 1);
                RenderBit((pixel >> 2) & 1);
                RenderBit((pixel >> 1) & 1);
                RenderBit(pixel & 1);
            }

            for (ushort beam = 0; beam < graphics.BytesPerRow; beam++)
            {
                int index = start + (byte)(beam + graphics.HorizontalOffset);
                byte character = memory[index];
                int yOffset = y % 12;

                if (character != 0 && character != 255 && character != 96)
                {
                    
                }

                switch (character >> 6)
                {
                    case 0:
                        character &= 63;
                        textPalette[0] = palette[graphics.TextBgPalette];
                        textPalette[1] = palette[graphics.TextFgPalette];

                        pixel = graphics.LowerCase && character < 32
                            ? Fonts.NTSCRoundFontData8x12[(character + 80) * 12 + yOffset]
                            : (byte)~Fonts.NTSCRoundFontData8x12[character * 12 + yOffset];

                        break;

                    case 1:
                        character &= 63;
                        textPalette[0] = palette[graphics.TextBgPalette];
                        textPalette[1] = palette[graphics.TextFgPalette];
                        pixel = Fonts.NTSCRoundFontData8x12[character * 12 + yOffset];
                        break;

                    case 2:
                    case 3:
                        textPalette[1] = palette[(character & 112) >> 4];
                        textPalette[0] = palette[8];
                        character = (byte)(64 + (character & 0xF));
                        pixel = Fonts.NTSCRoundFontData8x12[character * 12 + yOffset];
                        break;
                }

                Render();

                if (!emu.ScanLines)
                {
                    yStride -= 16;
                    yStride += xPitch;

                    Render();

                    yStride -= xPitch;
                }
            }
        }
    }
}
