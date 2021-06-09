using VCCSharp.Models;

namespace VCCSharp.Modules.TC1014
{
    // ReSharper disable once InconsistentNaming
    public partial class TC1014
    {
        public unsafe void SwitchMasterMode32(byte masterMode, uint start, uint yStride)
        {
            byte pixel = 0;
            byte character = 0, attributes = 0;
            uint[] textPalette = { 0, 0 };
            ushort widePixel = 0;
            byte pix = 0, bit = 0, phase = 0;
            byte carry1 = 1, carry2 = 0;
            byte color = 0;

            GraphicsSurfaces graphicsSurfaces = Graphics.GetGraphicsSurfaces();
            GraphicsColors graphicsColors = Graphics.GetGraphicsColors();

            byte* ramBuffer = Memory;
            ushort* wRamBuffer = (ushort*)ramBuffer;

            uint* szSurface32 = graphicsSurfaces.pSurface32;

            uint xPitch = (uint)(_modules.Emu.SurfacePitch);
            ushort y = (ushort)(_modules.Emu.LineCounter);

            switch (masterMode) // (GraphicsMode <<7) | (CompatMode<<6)  | ((Bpp & 3)<<4) | (Stretch & 15);
            {
                #region case 0  //Width 80

                case 0: //Width 80
                    attributes = 0;

                    if ((Graphics.HorizontalOffsetReg & 128) != 0)
                    {
                        start = (uint)(Graphics.StartOfVidRam + (Graphics.TagY / Graphics.LinesPerRow) * (Graphics.VPitch)); //Fix for Horizontal Offset Register in text mode.
                    }

                    for (ushort beam = 0; beam < Graphics.BytesPerRow * Graphics.ExtendedText; beam += Graphics.ExtendedText)
                    {
                        character = ramBuffer[start + (byte)(beam + Graphics.HorizontalOffset)];
                        pixel = CC3FontData8X12[character * 12 + (y % Graphics.LinesPerRow)];

                        if (Graphics.ExtendedText == 2)
                        {
                            attributes = ramBuffer[start + (byte)(beam + Graphics.HorizontalOffset) + 1];

                            if (((attributes & 64) != 0) && (y % Graphics.LinesPerRow == (Graphics.LinesPerRow - 1)))
                            {   //UnderLine
                                pixel = 255;
                            }

                            if (Graphics.CheckState(attributes))
                            {
                                pixel = 0;
                            }
                        }

                        textPalette[1] = graphicsColors.Palette32Bit[8 + ((attributes & 56) >> 3)];
                        textPalette[0] = graphicsColors.Palette32Bit[attributes & 7];
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

                    break;
                #endregion

                #region case 1-2  //Width 40

                case 1:
                case 2: //Width 40
                    attributes = 0;

                    for (ushort beam = 0; beam < Graphics.BytesPerRow * Graphics.ExtendedText; beam += Graphics.ExtendedText)
                    {
                        character = ramBuffer[start + (byte)(beam + Graphics.HorizontalOffset)];
                        pixel = CC3FontData8X12[character * 12 + (y % Graphics.LinesPerRow)];

                        if (Graphics.ExtendedText == 2)
                        {
                            attributes = ramBuffer[start + (byte)(beam + Graphics.HorizontalOffset) + 1];

                            if (((attributes & 64) != 0) && (y % Graphics.LinesPerRow == (Graphics.LinesPerRow - 1)))
                            {   //UnderLine
                                pixel = 255;
                            }

                            if (Graphics.CheckState(attributes))
                            {
                                pixel = 0;
                            }
                        }

                        textPalette[1] = graphicsColors.Palette32Bit[8 + ((attributes & 56) >> 3)];
                        textPalette[0] = graphicsColors.Palette32Bit[attributes & 7];
                        szSurface32[yStride += 1] = textPalette[pixel >> 7];
                        szSurface32[yStride += 1] = textPalette[pixel >> 7];
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
                            szSurface32[yStride += 1] = textPalette[pixel >> 7];
                            szSurface32[yStride += 1] = textPalette[pixel >> 7];
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

                    break;
                #endregion

                #region case 3-63

                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                case 30:
                case 31:
                case 32:
                case 33:
                case 34:
                case 35:
                case 36:
                case 37:
                case 38:
                case 39:
                case 40:
                case 41:
                case 42:
                case 43:
                case 44:
                case 45:
                case 46:
                case 47:
                case 48:
                case 49:
                case 50:
                case 51:
                case 52:
                case 53:
                case 54:
                case 55:
                case 56:
                case 57:
                case 58:
                case 59:
                case 60:
                case 61:
                case 62:
                case 63:
                    return; //TODO: Why?

                //for (ushort beam = 0; beam < Graphics.BytesPerRow * gs->ExtendedText; beam += gs->ExtendedText)
                //{
                //    character = ramBuffer[start + (byte)(beam + Graphics.HorizontalOffset)];

                //    if (gs->ExtendedText == 2)
                //    {
                //        attributes = ramBuffer[start + (byte)(beam + Graphics.HorizontalOffset) + 1];
                //    }
                //    else
                //    {
                //        attributes = 0;
                //    }

                //    pixel = _cc3FontData8x12[(character & 127) * 8 + (y % 8)];

                //    if (((attributes & 64) != 0) && (y % 8 == 7))
                //    {   //UnderLine
                //        pixel = 255;
                //    }

                //    if (Graphics.CheckState(attributes))
                //    {
                //        pixel = 0;
                //    }

                //    textPalette[1] = graphicsColors.Palette32Bit[8 + ((attributes & 56) >> 3)];
                //    textPalette[0] = graphicsColors.Palette32Bit[attributes & 7];
                //    szSurface32[yStride += 1] = textPalette[(pixel & 128) / 128];
                //    szSurface32[yStride += 1] = textPalette[(pixel & 64) / 64];
                //    szSurface32[yStride += 1] = textPalette[(pixel & 32) / 32];
                //    szSurface32[yStride += 1] = textPalette[(pixel & 16) / 16];
                //    szSurface32[yStride += 1] = textPalette[(pixel & 8) / 8];
                //    szSurface32[yStride += 1] = textPalette[(pixel & 4) / 4];
                //    szSurface32[yStride += 1] = textPalette[(pixel & 2) / 2];
                //    szSurface32[yStride += 1] = textPalette[(pixel & 1)];
                //}
                //break;

                #endregion

                #region case 64-127 //Low Res Text

                //******************************************************************** Low Res Text
                case 64:        //Low Res text GraphicsMode=0 CompatMode=1 Ignore Bpp and Stretch
                case 65:
                case 66:
                case 67:
                case 68:
                case 69:
                case 70:
                case 71:
                case 72:
                case 73:
                case 74:
                case 75:
                case 76:
                case 77:
                case 78:
                case 79:
                case 80:
                case 81:
                case 82:
                case 83:
                case 84:
                case 85:
                case 86:
                case 87:
                case 88:
                case 89:
                case 90:
                case 91:
                case 92:
                case 93:
                case 94:
                case 95:
                case 96:
                case 97:
                case 98:
                case 99:
                case 100:
                case 101:
                case 102:
                case 103:
                case 104:
                case 105:
                case 106:
                case 107:
                case 108:
                case 109:
                case 110:
                case 111:
                case 112:
                case 113:
                case 114:
                case 115:
                case 116:
                case 117:
                case 118:
                case 119:
                case 120:
                case 121:
                case 122:
                case 123:
                case 124:
                case 125:
                case 126:
                case 127:
                    for (ushort beam = 0; beam < Graphics.BytesPerRow; beam++)
                    {
                        character = ramBuffer[start + (byte)(beam + Graphics.HorizontalOffset)];

                        switch ((character & 192) >> 6)
                        {
                            case 0:
                                character &= 63;
                                textPalette[0] = graphicsColors.Palette32Bit[Graphics.TextBgPalette];
                                textPalette[1] = graphicsColors.Palette32Bit[Graphics.TextFgPalette];

                                if ((Graphics.LowerCase != 0) && (character < 32))
                                    pixel = NTSCRoundFontData8x12[(character + 80) * 12 + (y % 12)];
                                else
                                    pixel = (byte)~NTSCRoundFontData8x12[(character) * 12 + (y % 12)];
                                break;

                            case 1:
                                character &= 63;
                                textPalette[0] = graphicsColors.Palette32Bit[Graphics.TextBgPalette];
                                textPalette[1] = graphicsColors.Palette32Bit[Graphics.TextFgPalette];
                                pixel = NTSCRoundFontData8x12[(character) * 12 + (y % 12)];
                                break;

                            case 2:
                            case 3:
                                textPalette[1] = graphicsColors.Palette32Bit[(character & 112) >> 4];
                                textPalette[0] = graphicsColors.Palette32Bit[8];
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

                    break;

                #endregion

                #region case 128 + 0 //Bpp=0 Sr=0 1BPP Stretch=1

                case 128 + 0:
                    for (ushort beam = 0; beam < Graphics.BytesPerRow; beam += 2) //1bbp Stretch=1
                    {
                        widePixel = wRamBuffer[(Graphics.VidMask & (start + (byte)(Graphics.HorizontalOffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];

                        if (!_modules.Emu.ScanLines)
                        {
                            yStride -= (16);
                            yStride += xPitch;
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];
                            yStride -= xPitch;
                        }
                    }
                    break;

                #endregion

                #region case 128 + (1-2) //Bpp=0 Sr=1 1BPP Stretch=2

                case 128 + 1: //Bpp=0 Sr=1 1BPP Stretch=2
                case 128 + 2:   //Bpp=0 Sr=2 
                    for (ushort beam = 0; beam < Graphics.BytesPerRow; beam += 2) //1bbp Stretch=2
                    {
                        widePixel = wRamBuffer[(Graphics.VidMask & (start + (byte)(Graphics.HorizontalOffset + beam))) >> 1];

                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];

                        if (!_modules.Emu.ScanLines)
                        {
                            yStride -= (32);
                            yStride += xPitch;
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];
                            yStride -= xPitch;
                        }
                    }
                    break;

                #endregion

                #region case 128 + (3-6) //Bpp=0 Sr=3 1BPP Stretch=4

                case 128 + 3: //Bpp=0 Sr=3 1BPP Stretch=4
                case 128 + 4: //Bpp=0 Sr=4
                case 128 + 5: //Bpp=0 Sr=5
                case 128 + 6: //Bpp=0 Sr=6
                    for (ushort beam = 0; beam < Graphics.BytesPerRow; beam += 2) //1bbp Stretch=4
                    {
                        widePixel = wRamBuffer[(Graphics.VidMask & (start + (byte)(Graphics.HorizontalOffset + beam))) >> 1];

                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];

                        if (!_modules.Emu.ScanLines)
                        {
                            yStride -= (64);
                            yStride += xPitch;
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];
                            yStride -= xPitch;
                        }
                    }
                    break;

                #endregion

                #region case 128 + (7-14) //Bpp=0 Sr=7 1BPP Stretch=8 

                case 128 + 7: //Bpp=0 Sr=7 1BPP Stretch=8 
                case 128 + 8: //Bpp=0 Sr=8
                case 128 + 9: //Bpp=0 Sr=9
                case 128 + 10: //Bpp=0 Sr=10
                case 128 + 11: //Bpp=0 Sr=11
                case 128 + 12: //Bpp=0 Sr=12
                case 128 + 13: //Bpp=0 Sr=13
                case 128 + 14: //Bpp=0 Sr=14
                    for (ushort beam = 0; beam < Graphics.BytesPerRow; beam += 2) //1bbp Stretch=8
                    {
                        widePixel = wRamBuffer[(Graphics.VidMask & (start + (byte)(Graphics.HorizontalOffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];

                        if (!_modules.Emu.ScanLines)
                        {
                            yStride -= (128);
                            yStride += xPitch;
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 7)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 5)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 3)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 1)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 15)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 13)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 11)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 9)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[1 & (widePixel >> 8)];
                            yStride -= xPitch;
                        }
                    }
                    break;

                #endregion

                #region case 128 + (15-16) //Bpp=0 Sr=15 1BPP Stretch=16

                case 128 + 15: //Bpp=0 Sr=15 1BPP Stretch=16
                case 128 + 16: //BPP=1 Sr=0  2BPP Stretch=1
                    for (ushort beam = 0; beam < Graphics.BytesPerRow; beam += 2) //2bbp Stretch=1
                    {
                        widePixel = wRamBuffer[(Graphics.VidMask & (start + (byte)(Graphics.HorizontalOffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];

                        if (!_modules.Emu.ScanLines)
                        {
                            yStride -= (8);
                            yStride += xPitch;
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            yStride -= xPitch;
                        }
                    }
                    break;

                #endregion

                #region case 128 + (17-18) //Bpp=1 Sr=1  2BPP Stretch=2

                case 128 + 17: //Bpp=1 Sr=1  2BPP Stretch=2
                case 128 + 18: //Bpp=1 Sr=2
                    for (ushort beam = 0; beam < Graphics.BytesPerRow; beam += 2) //2bbp Stretch=2
                    {
                        widePixel = wRamBuffer[(Graphics.VidMask & (start + (byte)(Graphics.HorizontalOffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];

                        if (!_modules.Emu.ScanLines)
                        {
                            yStride -= (16);
                            yStride += xPitch;
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            yStride -= xPitch;
                        }
                    }

                    break;

                #endregion

                #region case 128 + (19-22) //Bpp=1 Sr=3  2BPP Stretch=4

                case 128 + 19: //Bpp=1 Sr=3  2BPP Stretch=4
                case 128 + 20: //Bpp=1 Sr=4
                case 128 + 21: //Bpp=1 Sr=5
                case 128 + 22: //Bpp=1 Sr=6
                    for (ushort beam = 0; beam < Graphics.BytesPerRow; beam += 2) //2bbp Stretch=4
                    {
                        widePixel = wRamBuffer[(Graphics.VidMask & (start + (byte)(Graphics.HorizontalOffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];

                        if (!_modules.Emu.ScanLines)
                        {
                            yStride -= (32);
                            yStride += xPitch;
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            yStride -= xPitch;
                        }
                    }
                    break;

                #endregion

                #region case 128 + (23-30) //Bpp=1 Sr=7  2BPP Stretch=8

                case 128 + 23: //Bpp=1 Sr=7  2BPP Stretch=8
                case 128 + 24: //Bpp=1 Sr=8
                case 128 + 25: //Bpp=1 Sr=9 
                case 128 + 26: //Bpp=1 Sr=10 
                case 128 + 27: //Bpp=1 Sr=11 
                case 128 + 28: //Bpp=1 Sr=12 
                case 128 + 29: //Bpp=1 Sr=13 
                case 128 + 30: //Bpp=1 Sr=14
                    for (ushort beam = 0; beam < Graphics.BytesPerRow; beam += 2) //2bbp Stretch=8
                    {
                        widePixel = wRamBuffer[(Graphics.VidMask & (start + (byte)(Graphics.HorizontalOffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];

                        if (!_modules.Emu.ScanLines)
                        {
                            yStride -= (64);
                            yStride += xPitch;
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            yStride -= xPitch;
                        }
                    }
                    break;

                #endregion

                #region case 128 + 31 //Bpp=1 Sr=15 2BPP Stretch=16 

                case 128 + 31: //Bpp=1 Sr=15 2BPP Stretch=16 
                    for (ushort beam = 0; beam < Graphics.BytesPerRow; beam += 2) //2bbp Stretch=16
                    {
                        widePixel = wRamBuffer[(Graphics.VidMask & (start + (byte)(Graphics.HorizontalOffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];

                        if (!_modules.Emu.ScanLines)
                        {
                            yStride -= (128);
                            yStride += xPitch;
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 6)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 2)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 14)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 10)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[3 & (widePixel >> 8)];
                            yStride -= xPitch;
                        }
                    }
                    break;

                #endregion

                #region case 128 + 32 //Bpp=2 Sr=0 4BPP Stretch=1

                case 128 + 32: //Bpp=2 Sr=0 4BPP Stretch=1
                    for (ushort beam = 0; beam < Graphics.BytesPerRow; beam += 2) //4bbp Stretch=1
                    {
                        widePixel = wRamBuffer[(Graphics.VidMask & (start + (byte)(Graphics.HorizontalOffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];

                        if (!_modules.Emu.ScanLines)
                        {
                            yStride -= (4);
                            yStride += xPitch;
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            yStride -= xPitch;
                        }
                    }
                    break;

                #endregion

                #region case 128 + (33-34) //Bpp=2 Sr=1 4BPP Stretch=2 

                case 128 + 33: //Bpp=2 Sr=1 4BPP Stretch=2 
                case 128 + 34: //Bpp=2 Sr=2
                    for (ushort beam = 0; beam < Graphics.BytesPerRow; beam += 2) //4bbp Stretch=2
                    {
                        widePixel = wRamBuffer[(Graphics.VidMask & (start + (byte)(Graphics.HorizontalOffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];

                        if (!_modules.Emu.ScanLines)
                        {
                            yStride -= (8);
                            yStride += xPitch;
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            yStride -= xPitch;
                        }
                    }
                    break;

                #endregion

                #region case 128 + (35-38) //Bpp=2 Sr=3 4BPP Stretch=4

                case 128 + 35: //Bpp=2 Sr=3 4BPP Stretch=4
                case 128 + 36: //Bpp=2 Sr=4 
                case 128 + 37: //Bpp=2 Sr=5 
                case 128 + 38: //Bpp=2 Sr=6 
                    for (ushort beam = 0; beam < Graphics.BytesPerRow; beam += 2) //4bbp Stretch=4
                    {
                        widePixel = wRamBuffer[(Graphics.VidMask & (start + (byte)(Graphics.HorizontalOffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];

                        if (!_modules.Emu.ScanLines)
                        {
                            yStride -= (16);
                            yStride += xPitch;
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            yStride -= xPitch;
                        }
                    }
                    break;

                #endregion

                #region case 12 + (39-46) //Bpp=2 Sr=7 4BPP Stretch=8

                case 128 + 39: //Bpp=2 Sr=7 4BPP Stretch=8
                case 128 + 40: //Bpp=2 Sr=8 
                case 128 + 41: //Bpp=2 Sr=9 
                case 128 + 42: //Bpp=2 Sr=10 
                case 128 + 43: //Bpp=2 Sr=11 
                case 128 + 44: //Bpp=2 Sr=12 
                case 128 + 45: //Bpp=2 Sr=13 
                case 128 + 46: //Bpp=2 Sr=14 
                    for (ushort beam = 0; beam < Graphics.BytesPerRow; beam += 2) //4bbp Stretch=8
                    {
                        widePixel = wRamBuffer[(Graphics.VidMask & (start + (byte)(Graphics.HorizontalOffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];

                        if (!_modules.Emu.ScanLines)
                        {
                            yStride -= (32);
                            yStride += xPitch;
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            yStride -= xPitch;
                        }
                    }
                    break;

                #endregion

                #region case 128 + 47: //Bpp=2 Sr=15 4BPP Stretch=16

                case 128 + 47: //Bpp=2 Sr=15 4BPP Stretch=16
                    for (ushort beam = 0; beam < Graphics.BytesPerRow; beam += 2) //4bbp Stretch=16
                    {
                        widePixel = wRamBuffer[(Graphics.VidMask & (start + (byte)(Graphics.HorizontalOffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];

                        if (!_modules.Emu.ScanLines)
                        {
                            yStride -= (64);
                            yStride += xPitch;
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 4)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & widePixel];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 12)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[15 & (widePixel >> 8)];
                            yStride -= xPitch;
                        }
                    }
                    break;

                #endregion

                #region case 128 + (48-63) //Bpp=3 Sr=0  Unsupported 

                case 128 + 48: //Bpp=3 Sr=0  Unsupported 
                case 128 + 49: //Bpp=3 Sr=1 
                case 128 + 50: //Bpp=3 Sr=2 
                case 128 + 51: //Bpp=3 Sr=3 
                case 128 + 52: //Bpp=3 Sr=4 
                case 128 + 53: //Bpp=3 Sr=5 
                case 128 + 54: //Bpp=3 Sr=6 
                case 128 + 55: //Bpp=3 Sr=7 
                case 128 + 56: //Bpp=3 Sr=8 
                case 128 + 57: //Bpp=3 Sr=9 
                case 128 + 58: //Bpp=3 Sr=10 
                case 128 + 59: //Bpp=3 Sr=11 
                case 128 + 60: //Bpp=3 Sr=12 
                case 128 + 61: //Bpp=3 Sr=13 
                case 128 + 62: //Bpp=3 Sr=14 
                case 128 + 63: //Bpp=3 Sr=15 
                    return;

                #endregion

                #region case 192 + 0: //Bpp=0 Sr=0 1BPP Stretch=1

                case 192 + 0: //Bpp=0 Sr=0 1BPP Stretch=1
                    for (ushort beam = 0; beam < Graphics.BytesPerRow; beam += 2) //1bbp Stretch=1
                    {
                        widePixel = wRamBuffer[(Graphics.VidMask & (start + (byte)(Graphics.HorizontalOffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];

                        if (!_modules.Emu.ScanLines)
                        {
                            yStride -= (16);
                            yStride += xPitch;
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];
                            yStride -= xPitch;
                        }
                    }
                    break;

                #endregion

                #region case 192 + (1-2): //Bpp=0 Sr=1 1BPP Stretch=2

                case 192 + 1: //Bpp=0 Sr=1 1BPP Stretch=2
                case 192 + 2:   //Bpp=0 Sr=2 
                    for (ushort beam = 0; beam < Graphics.BytesPerRow; beam += 2) //1bbp Stretch=2
                    {
                        widePixel = wRamBuffer[(Graphics.VidMask & (start + (byte)(Graphics.HorizontalOffset + beam))) >> 1];

                        if (Graphics.MonitorType == 0)
                        { //Pcolor
                            for (byte xbit = 8; xbit > 0; xbit--)
                            {
                                bit = (byte)(xbit - 1); //TODO: work-around that a byte is always positive
                                pix = (byte)(1 & (widePixel >> bit));
                                phase = (byte)((carry2 << 2) | (carry1 << 1) | pix);

                                switch (phase)
                                {
                                    case 0:
                                    case 4:
                                    case 6:
                                        color = 0;
                                        break;

                                    case 1:
                                    case 5:
                                        color = (byte)((bit & 1) + 1);
                                        break;

                                    case 2:
                                        break;

                                    case 3:
                                        color = 3;

                                        int colorInvert3 = Graphics.ColorInvert ? 7 : 3;// * 4 + 3;

                                        szSurface32[yStride - 1] = graphicsColors.Afacts32[colorInvert3];

                                        if (!_modules.Emu.ScanLines)
                                        {
                                            szSurface32[yStride + xPitch - 1] = graphicsColors.Afacts32[colorInvert3];
                                        }

                                        szSurface32[yStride] = graphicsColors.Afacts32[colorInvert3];

                                        if (!_modules.Emu.ScanLines)
                                        {
                                            szSurface32[yStride + xPitch] = graphicsColors.Afacts32[colorInvert3];
                                        }

                                        break;

                                    case 7:
                                        color = 3;
                                        break;
                                }

                                int colorInvert = (Graphics.ColorInvert ? 4 : 0) + color;

                                szSurface32[yStride += 1] = graphicsColors.Afacts32[colorInvert];

                                if (!_modules.Emu.ScanLines)
                                {
                                    szSurface32[yStride + xPitch] = graphicsColors.Afacts32[colorInvert];
                                }

                                szSurface32[yStride += 1] = graphicsColors.Afacts32[colorInvert];

                                if (!_modules.Emu.ScanLines)
                                {
                                    szSurface32[yStride + xPitch] = graphicsColors.Afacts32[colorInvert];
                                }

                                carry2 = carry1;
                                carry1 = pix;
                            }

                            for (bit = 15; bit >= 8; bit--)
                            {
                                pix = (byte)(1 & (widePixel >> bit));
                                phase = (byte)((carry2 << 2) | (carry1 << 1) | pix);

                                switch (phase)
                                {
                                    case 0:
                                    case 4:
                                    case 6:
                                        color = 0;
                                        break;

                                    case 1:
                                    case 5:
                                        color = (byte)((bit & 1) + 1);
                                        break;

                                    case 2:
                                        break;

                                    case 3:
                                        color = 3;

                                        int colorInvert3 = Graphics.ColorInvert ? 7 : 3; //* 4 + 3;

                                        szSurface32[yStride - 1] = graphicsColors.Afacts32[colorInvert3];

                                        if (!_modules.Emu.ScanLines)
                                        {
                                            szSurface32[yStride + xPitch - 1] = graphicsColors.Afacts32[colorInvert3];
                                        }

                                        szSurface32[yStride] = graphicsColors.Afacts32[colorInvert3];

                                        if (!_modules.Emu.ScanLines)
                                        {
                                            szSurface32[yStride + xPitch] = graphicsColors.Afacts32[colorInvert3];
                                        }

                                        break;

                                    case 7:
                                        color = 3;
                                        break;
                                }

                                int colorInvert = (Graphics.ColorInvert ? 4 : 0) + color;

                                szSurface32[yStride += 1] = graphicsColors.Afacts32[colorInvert];

                                if (!_modules.Emu.ScanLines)
                                {
                                    szSurface32[yStride + xPitch] = graphicsColors.Afacts32[colorInvert];
                                }

                                szSurface32[yStride += 1] = graphicsColors.Afacts32[colorInvert];

                                if (!_modules.Emu.ScanLines)
                                {
                                    szSurface32[yStride + xPitch] = graphicsColors.Afacts32[colorInvert];
                                }

                                carry2 = carry1;
                                carry1 = pix;
                            }
                        }
                        else
                        {
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];

                            if (!_modules.Emu.ScanLines)
                            {
                                yStride -= (32);
                                yStride += xPitch;
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];
                                szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];
                                yStride -= xPitch;
                            }
                        }
                    }
                    break;

                #endregion

                #region case 192 + (3-6): //Bpp=0 Sr=3 1BPP Stretch=4

                case 192 + 3: //Bpp=0 Sr=3 1BPP Stretch=4
                case 192 + 4: //Bpp=0 Sr=4
                case 192 + 5: //Bpp=0 Sr=5
                case 192 + 6: //Bpp=0 Sr=6
                    for (ushort beam = 0; beam < Graphics.BytesPerRow; beam += 2) //1bbp Stretch=4
                    {
                        widePixel = wRamBuffer[(Graphics.VidMask & (start + (byte)(Graphics.HorizontalOffset + beam))) >> 1];

                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];

                        if (!_modules.Emu.ScanLines)
                        {
                            yStride -= (64);
                            yStride += xPitch;
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];
                            yStride -= xPitch;
                        }
                    }
                    break;

                #endregion

                #region case 192 + (7-14): //Bpp=0 Sr=7 1BPP Stretch=8 

                case 192 + 7: //Bpp=0 Sr=7 1BPP Stretch=8 
                case 192 + 8: //Bpp=0 Sr=8
                case 192 + 9: //Bpp=0 Sr=9
                case 192 + 10: //Bpp=0 Sr=10
                case 192 + 11: //Bpp=0 Sr=11
                case 192 + 12: //Bpp=0 Sr=12
                case 192 + 13: //Bpp=0 Sr=13
                case 192 + 14: //Bpp=0 Sr=14
                    for (ushort beam = 0; beam < Graphics.BytesPerRow; beam += 2) //1bbp Stretch=8
                    {
                        widePixel = wRamBuffer[(Graphics.VidMask & (start + (byte)(Graphics.HorizontalOffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];

                        if (!_modules.Emu.ScanLines)
                        {
                            yStride -= (128);
                            yStride += xPitch;
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 7))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 5))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 3))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 1))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 15))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 13))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 11))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 9))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (1 & (widePixel >> 8))];
                            yStride -= xPitch;
                        }
                    }
                    break;

                #endregion

                #region case 192 + (15-16): //Bpp=0 Sr=15 1BPP Stretch=16

                case 192 + 15: //Bpp=0 Sr=15 1BPP Stretch=16
                case 192 + 16: //BPP=1 Sr=0  2BPP Stretch=1
                    for (ushort beam = 0; beam < Graphics.BytesPerRow; beam += 2) //2bbp Stretch=1
                    {
                        widePixel = wRamBuffer[(Graphics.VidMask & (start + (byte)(Graphics.HorizontalOffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];

                        if (!_modules.Emu.ScanLines)
                        {
                            yStride -= (8);
                            yStride += xPitch;
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            yStride -= xPitch;
                        }
                    }
                    break;

                #endregion

                #region case 192 + (17-18): //Bpp=1 Sr=1  2BPP Stretch=2

                case 192 + 17: //Bpp=1 Sr=1  2BPP Stretch=2
                case 192 + 18: //Bpp=1 Sr=2
                    for (ushort beam = 0; beam < Graphics.BytesPerRow; beam += 2) //2bbp Stretch=2
                    {
                        widePixel = wRamBuffer[(Graphics.VidMask & (start + (byte)(Graphics.HorizontalOffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];

                        if (!_modules.Emu.ScanLines)
                        {
                            yStride -= (16);
                            yStride += xPitch;
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            yStride -= xPitch;
                        }
                    }
                    break;


                #endregion

                #region case 192 + (19-22): //Bpp=1 Sr=3  2BPP Stretch=4

                case 192 + 19: //Bpp=1 Sr=3  2BPP Stretch=4
                case 192 + 20: //Bpp=1 Sr=4
                case 192 + 21: //Bpp=1 Sr=5
                case 192 + 22: //Bpp=1 Sr=6
                    for (ushort beam = 0; beam < Graphics.BytesPerRow; beam += 2) //2bbp Stretch=4
                    {
                        widePixel = wRamBuffer[(Graphics.VidMask & (start + (byte)(Graphics.HorizontalOffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];

                        if (!_modules.Emu.ScanLines)
                        {
                            yStride -= (32);
                            yStride += xPitch;
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            yStride -= xPitch;
                        }
                    }
                    break;

                #endregion

                #region case 192 + (23-30): //Bpp=1 Sr=7  2BPP Stretch=8

                case 192 + 23: //Bpp=1 Sr=7  2BPP Stretch=8
                case 192 + 24: //Bpp=1 Sr=8
                case 192 + 25: //Bpp=1 Sr=9 
                case 192 + 26: //Bpp=1 Sr=10 
                case 192 + 27: //Bpp=1 Sr=11 
                case 192 + 28: //Bpp=1 Sr=12 
                case 192 + 29: //Bpp=1 Sr=13 
                case 192 + 30: //Bpp=1 Sr=14
                    for (ushort beam = 0; beam < Graphics.BytesPerRow; beam += 2) //2bbp Stretch=8
                    {
                        widePixel = wRamBuffer[(Graphics.VidMask & (start + (byte)(Graphics.HorizontalOffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];

                        if (!_modules.Emu.ScanLines)
                        {
                            yStride -= (64);
                            yStride += xPitch;
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            yStride -= xPitch;
                        }
                    }
                    break;


                #endregion

                #region case 192 + 31: //Bpp=1 Sr=15 2BPP Stretch=16 

                case 192 + 31: //Bpp=1 Sr=15 2BPP Stretch=16 
                    for (ushort beam = 0; beam < Graphics.BytesPerRow; beam += 2) //2bbp Stretch=16
                    {
                        widePixel = wRamBuffer[(Graphics.VidMask & (start + (byte)(Graphics.HorizontalOffset + beam))) >> 1];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];

                        if (!_modules.Emu.ScanLines)
                        {
                            yStride -= (128);
                            yStride += xPitch;
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 6))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 4))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 2))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & widePixel)];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 14))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 12))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 10))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            szSurface32[yStride += 1] = graphicsColors.Palette32Bit[Graphics.PaletteIndex + (3 & (widePixel >> 8))];
                            yStride -= xPitch;
                        }
                    }
                    break;


                #endregion

                #region case 192 + (32-63): //Bpp=2 Sr=0 4BPP Stretch=1 Unsupport with Compat set

                case 192 + 32: //Bpp=2 Sr=0 4BPP Stretch=1 Unsupport with Compat set
                case 192 + 33: //Bpp=2 Sr=1 4BPP Stretch=2 
                case 192 + 34: //Bpp=2 Sr=2
                case 192 + 35: //Bpp=2 Sr=3 4BPP Stretch=4
                case 192 + 36: //Bpp=2 Sr=4 
                case 192 + 37: //Bpp=2 Sr=5 
                case 192 + 38: //Bpp=2 Sr=6 
                case 192 + 39: //Bpp=2 Sr=7 4BPP Stretch=8
                case 192 + 40: //Bpp=2 Sr=8 
                case 192 + 41: //Bpp=2 Sr=9 
                case 192 + 42: //Bpp=2 Sr=10 
                case 192 + 43: //Bpp=2 Sr=11 
                case 192 + 44: //Bpp=2 Sr=12 
                case 192 + 45: //Bpp=2 Sr=13 
                case 192 + 46: //Bpp=2 Sr=14 
                case 192 + 47: //Bpp=2 Sr=15 4BPP Stretch=16
                case 192 + 48: //Bpp=3 Sr=0  Unsupported 
                case 192 + 49: //Bpp=3 Sr=1 
                case 192 + 50: //Bpp=3 Sr=2 
                case 192 + 51: //Bpp=3 Sr=3 
                case 192 + 52: //Bpp=3 Sr=4 
                case 192 + 53: //Bpp=3 Sr=5 
                case 192 + 54: //Bpp=3 Sr=6 
                case 192 + 55: //Bpp=3 Sr=7 
                case 192 + 56: //Bpp=3 Sr=8 
                case 192 + 57: //Bpp=3 Sr=9 
                case 192 + 58: //Bpp=3 Sr=10 
                case 192 + 59: //Bpp=3 Sr=11 
                case 192 + 60: //Bpp=3 Sr=12 
                case 192 + 61: //Bpp=3 Sr=13 
                case 192 + 62: //Bpp=3 Sr=14 
                case 192 + 63: //Bpp=3 Sr=15 
                    return;


                    #endregion
            }
        }
    }
}
