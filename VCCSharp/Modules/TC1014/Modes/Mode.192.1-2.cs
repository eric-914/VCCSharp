namespace VCCSharp.Modules.TC1014.Modes
{
    // ReSharper disable once InconsistentNaming
    public static class _192_1_2
    {
        //Bpp=0 Sr=1 1BPP Stretch=2
        //Bpp=0 Sr=2 
        public static unsafe void Mode(ModeModel model, int start, int yStride)
        {
            byte carry1 = 1, carry2 = 0;
            byte color = 0;

            IGraphics graphics = model.Modules.Graphics;
            IEmu emu = model.Modules.Emu;

            uint[] palette = graphics.GetGraphicsColors().Palette32Bit;
            uint[] artifacts = graphics.GetGraphicsColors().Afacts32;
            uint* szSurface32 = graphics.GetGraphicsSurfaces().pSurface32;
            ushort* wMemory = (ushort*) model.Memory;
            int xPitch = (int)emu.SurfacePitch;

            for (ushort beam = 0; beam < graphics.BytesPerRow; beam += 2) //1bbp Stretch=2
            {
                uint widePixel = wMemory[(graphics.VidMask & (start + (byte)(graphics.HorizontalOffset + beam))) >> 1];

                if (graphics.MonitorType == 0)
                { //Pcolor
                    for (byte xBit = 8; xBit > 0; xBit--)
                    {
                        byte bit = (byte)(xBit - 1); //TODO: work-around that a byte is always positive
                        byte pix = (byte)(1 & (widePixel >> bit));
                        byte phase = (byte)((carry2 << 2) | (carry1 << 1) | pix);

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

                                int colorInvert3 = graphics.ColorInvert ? 7 : 3;// * 4 + 3;

                                szSurface32[yStride - 1] = artifacts[colorInvert3];

                                if (!emu.ScanLines)
                                {
                                    szSurface32[yStride + xPitch - 1] = artifacts[colorInvert3];
                                }

                                szSurface32[yStride] = artifacts[colorInvert3];

                                if (!emu.ScanLines)
                                {
                                    szSurface32[yStride + xPitch] = artifacts[colorInvert3];
                                }

                                break;

                            case 7:
                                color = 3;
                                break;
                        }

                        int colorInvert = (graphics.ColorInvert ? 4 : 0) + color;

                        szSurface32[yStride += 1] = artifacts[colorInvert];

                        if (!emu.ScanLines)
                        {
                            szSurface32[yStride + xPitch] = artifacts[colorInvert];
                        }

                        szSurface32[yStride += 1] = artifacts[colorInvert];

                        if (!emu.ScanLines)
                        {
                            szSurface32[yStride + xPitch] = artifacts[colorInvert];
                        }

                        carry2 = carry1;
                        carry1 = pix;
                    }

                    for (byte bit = 15; bit >= 8; bit--)
                    {
                        byte pix = (byte)(1 & (widePixel >> bit));
                        byte phase = (byte)((carry2 << 2) | (carry1 << 1) | pix);

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

                                int colorInvert3 = graphics.ColorInvert ? 7 : 3; //* 4 + 3;

                                szSurface32[yStride - 1] = artifacts[colorInvert3];

                                if (!emu.ScanLines)
                                {
                                    szSurface32[yStride + xPitch - 1] = artifacts[colorInvert3];
                                }

                                szSurface32[yStride] = artifacts[colorInvert3];

                                if (!emu.ScanLines)
                                {
                                    szSurface32[yStride + xPitch] = artifacts[colorInvert3];
                                }

                                break;

                            case 7:
                                color = 3;
                                break;
                        }

                        int colorInvert = (graphics.ColorInvert ? 4 : 0) + color;

                        szSurface32[yStride += 1] = artifacts[colorInvert];

                        if (!emu.ScanLines)
                        {
                            szSurface32[yStride + xPitch] = artifacts[colorInvert];
                        }

                        szSurface32[yStride += 1] = artifacts[colorInvert];

                        if (!emu.ScanLines)
                        {
                            szSurface32[yStride + xPitch] = artifacts[colorInvert];
                        }

                        carry2 = carry1;
                        carry1 = pix;
                    }
                }
                else
                {
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 7))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 7))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 6))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 6))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 5))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 5))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 4))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 4))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 3))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 3))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 2))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 2))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 1))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 1))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & widePixel)];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & widePixel)];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 15))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 15))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 14))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 14))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 13))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 13))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 12))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 12))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 11))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 11))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 10))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 10))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 9))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 9))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 8))];
                    szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 8))];

                    if (!emu.ScanLines)
                    {
                        yStride -= (32);
                        yStride += xPitch;
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 7))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 6))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 5))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 4))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 3))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 2))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 1))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & widePixel)];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 15))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 14))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 13))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 12))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 11))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 10))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 9))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 8))];
                        szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (1 & (widePixel >> 8))];
                        yStride -= xPitch;
                    }
                }
            }
        }
    }
}
