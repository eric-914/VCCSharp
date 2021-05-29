using System;
using System.Diagnostics;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IGraphics
    {
        GraphicsSurfaces GetGraphicsSurfaces();
        GraphicsColors GetGraphicsColors();

        void ResetGraphicsState();
        void MakeRgbPalette();
        void MakeCmpPalette(int paletteType);
        void SetBlinkState(byte state);
        void SetBorderChange();
        void SetVidMask(uint mask);
        void SetPaletteType();
        void SetScanLines(byte lines);
        void SetMonitorType(byte type);
        void FlipArtifacts();
        void InvalidateBorder();
        bool CheckState(byte attributes);
        void SetGimeVdgMode(byte vdgMode);
        void SetGimeVdgOffset(byte offset);
        void SetGimeVmode(byte mode);
        void SetGimeVres(byte res);
        void SetGimeBorderColor(byte data);
        void SetVerticalOffsetRegister(ushort voRegister);
        void SetGimeHorizontalOffset(byte data);
        void SetGimePalette(byte palette, byte color);
        void SetCompatMode(byte mode);
        void SetVideoBank(byte data);
        void SetGimeVdgMode2(byte mode);
        unsafe void SetGraphicsSurfaces(void* ddsdGetSurface);

        byte BorderChange { get; set; }
        byte BytesPerRow { get; set; }
        byte ColorInvert { get; set; }
        byte ExtendedText { get; set; }
        byte GraphicsMode { get; set; }
        byte HorizontalOffset { get; set; }
        byte HorizontalCenter { get; set; }
        byte HorizontalOffsetReg { get; set; }
        byte LinesPerRow { get; set; }
        byte LinesPerScreen { get; set; }
        byte LowerCase { get; set; }
        byte MasterMode { get; set; }
        byte MonType { get; set; }
        byte PaletteIndex { get; set; }
        byte Stretch { get; set; }
        byte TextBgPalette { get; set; }
        byte TextFgPalette { get; set; }
        byte VerticalCenter { get; set; }
        ushort PixelsPerLine { get; set; }
        ushort TagY { get; set; }
        ushort VPitch { get; set; }
        uint NewStartOfVidRam { get; set; }
        uint StartOfVidRam { get; set; }
        uint VidMask { get; set; }
        byte BorderColor8 { get; set; }
        ushort BorderColor16 { get; set; }
        uint BorderColor32 { get; set; }

        byte[] Lpf { get; }
        byte[] VcenterTable { get; }
    }

    public class Graphics : IGraphics
    {
        private readonly int[] _red =
        {
            0, 14, 12, 21, 51, 86, 108, 118, 113, 92, 61, 21, 1, 5, 12, 13,
            50, 29, 49, 86, 119, 158, 179, 192, 186, 165, 133, 94, 23, 16, 23, 25,
            116, 74, 102, 142, 179, 219, 243, 252, 251, 230, 198, 155, 81, 61, 52, 57,
            253, 137, 161, 189, 215, 240, 253, 253, 251, 237, 214, 183, 134, 121, 116, 255
        };

        private readonly int[] _green =
        {
            0, 78, 69, 53, 33, 4, 1, 1, 12, 24, 31, 35, 37, 51, 67, 77,
            50, 149, 141, 123, 103, 77, 55, 39, 35, 43, 53, 63, 100, 119, 137, 148,
            116, 212, 204, 186, 164, 137, 114, 97, 88, 89, 96, 109, 156, 179, 199, 211,
            253, 230, 221, 207, 192, 174, 158, 148, 143, 144, 150, 162, 196, 212, 225, 255
        };

        private readonly int[] _blue =
        {
            0, 20, 18, 14, 10, 10, 12, 19, 76, 135, 178, 196, 148, 97, 29, 20,
            50, 38, 36, 32, 28, 25, 24, 78, 143, 207, 248, 249, 228, 174, 99, 46,
            116, 58, 52, 48, 44, 41, 68, 132, 202, 250, 250, 250, 251, 243, 163, 99,
            254, 104, 83, 77, 82, 105, 142, 188, 237, 251, 251, 251, 252, 240, 183, 255
        };

        private static readonly byte[] CoCo2Bpp = { 1, 0, 1, 0, 1, 0, 1, 0 };
        private static readonly byte[] CoCo2LinesPerRow = { 12, 3, 3, 2, 2, 1, 1, 1 };
        private static readonly byte[] CoCo3LinesPerRow = { 1, 1, 2, 8, 9, 10, 11, 200 };
        private static readonly byte[] CoCo2BytesPerRow = { 16, 16, 32, 16, 32, 16, 32, 32 };
        private static readonly byte[] CoCo3BytesPerRow = { 16, 20, 32, 40, 64, 80, 128, 160 };
        private static readonly byte[] CoCo3BytesPerTextRow = { 32, 40, 32, 40, 64, 80, 64, 80 };
        private static readonly byte[] CoCo2PaletteSet = { 8, 0, 10, 4 };
        private static readonly byte[] PixelsPerByte = { 8, 4, 2, 2 };

        private readonly IModules _modules;

        private readonly GraphicsColors _colors = new GraphicsColors();
        private static readonly GraphicsSurfaces Surfaces = new GraphicsSurfaces();

        public byte BlinkState { get; set; }
        public byte BorderChange { get; set; } = 3;
        public byte Bpp { get; set; }
        public byte BytesPerRow { get; set; } = 32;
        public byte CC2Offset { get; set; }
        public byte CC2VDGMode { get; set; }
        public byte CC2VDGPiaMode { get; set; }
        public byte CC3BorderColor { get; set; }
        public byte CC3Vmode { get; set; }
        public byte CC3Vres { get; set; }
        public byte ColorInvert { get; set; } = 1;
        public byte CompatMode { get; set; }
        public byte ExtendedText { get; set; } = 1;
        public byte GraphicsMode { get; set; }
        public byte HorizontalOffset { get; set; }
        public byte HorizontalCenter { get; set; }
        public byte HorizontalOffsetReg { get; set; }
        public byte InvertAll { get; set; }
        public byte LinesPerRow { get; set; } = 1;
        public byte LinesPerScreen { get; set; }
        public byte LowerCase { get; set; }
        public byte MasterMode { get; set; }
        public byte MonType { get; set; } = 1;
        public byte PaletteIndex { get; set; }
        public byte Stretch { get; set; }
        //public byte TextBGColor { get; set; }
        public byte TextBgPalette { get; set; }
        //public byte TextFGColor { get; set; }
        public byte TextFgPalette { get; set; }
        public byte VerticalCenter { get; set; }
        public byte VresIndex { get; set; }

        public ushort PixelsPerLine { get; set; }
        public ushort TagY { get; set; }
        public ushort VerticalOffsetRegister { get; set; }
        public ushort VPitch { get; set; } = 32;

        public uint DistoOffset { get; set; }
        public uint NewStartOfVidRam { get; set; }
        public uint StartOfVidRam { get; set; }
        public uint VidMask { get; set; } = 0x1FFFF;

        public byte BorderColor8 { get; set; }
        public ushort BorderColor16 { get; set; }
        public uint BorderColor32 { get; set; }

        public byte[] Lpf { get; } = { 192, 199, 225, 225 }; // #2 is really undefined but I gotta put something here.
        public byte[] VcenterTable { get; } = { 29, 23, 12, 12 };

        public Graphics(IModules modules)
        {
            _modules = modules;
        }

        public GraphicsSurfaces GetGraphicsSurfaces() => Surfaces;

        public GraphicsColors GetGraphicsColors() => _colors;

        public void ResetGraphicsState()
        {
            CC3Vmode = 0;
            CC3Vres = 0;
            StartOfVidRam = 0;
            NewStartOfVidRam = 0;
            GraphicsMode = 0;
            LowerCase = 0;
            InvertAll = 0;
            ExtendedText = 1;
            HorizontalOffsetReg = 0;
            TagY = 0;
            DistoOffset = 0;
            BorderChange = 3;
            CC2Offset = 0;
            HorizontalOffset = 0;
            VerticalOffsetRegister = 0;
        }

        public void SetBlinkState(byte state)
        {
            BlinkState = state;
        }

        public void SetBorderChange()
        {
            if (BorderChange > 0)
            {
                BorderChange--;
            }
        }

        public void SetVidMask(uint mask)
        {
            VidMask = mask;
        }

        public void FlipArtifacts()
        {
            ColorInvert = ColorInvert == Define.FALSE ? Define.TRUE : Define.FALSE;
        }

        //TODO: ScanLines never really worked right to begin with...
        public void SetScanLines(byte lines)
        {
            _modules.Emu.ScanLines = lines != Define.FALSE;
            _modules.Emu.ResetPending = (byte)ResetPendingStates.Cls;

            _modules.DirectDraw.ClearScreen();

            BorderChange = 3;
        }

        public void MakeCmpPalette(int paletteType)
        {
            Debug.WriteLine(paletteType == 1 ? "Loading new CMP palette." : "Loading old CMP palette.");

            float gamma = 1.4f;

            for (byte index = 0; index <= 63; index++)
            {
                double r, g, b;

                if (paletteType == 1)
                {
                    if (index > 39) { gamma = 1.1f; }

                    if (index > 55) { gamma = 1; }

                    r = _red[index] * (double)gamma; if (r > 255) { r = 255; }
                    g = _green[index] * (double)gamma; if (g > 255) { g = 255; }
                    b = _blue[index] * (double)gamma; if (b > 255) { b = 255; }
                }
                else //Old palette //Stolen from M.E.S.S.
                {
                    switch (index)
                    {
                        case 0:
                            r = g = b = 0;
                            break;

                        case 16:
                            r = g = b = 47;
                            break;

                        case 32:
                            r = g = b = 120;
                            break;

                        case 48:
                        case 63:
                            r = g = b = 255;
                            break;

                        default:
                            double w = .4195456981879 * 1.01;
                            double contrast = 70;
                            double saturation = 92;
                            double brightness = -20;
                            brightness += (((double)index / 16) + 1) * contrast;
                            int offset = (index % 16) - 1 + (index / 16) * 15;
                            r = Math.Cos(w * (offset + 9.2)) * saturation + brightness;
                            g = Math.Cos(w * (offset + 14.2)) * saturation + brightness;
                            b = Math.Cos(w * (offset + 19.2)) * saturation + brightness;

                            if (r < 0)
                            {
                                r = 0;
                            }
                            else if (r > 255)
                            {
                                r = 255;
                            }

                            if (g < 0)
                            {
                                g = 0;
                            }
                            else if (g > 255)
                            {
                                g = 255;
                            }

                            if (b < 0)
                            {
                                b = 0;
                            }
                            else if (b > 255)
                            {
                                b = 255;
                            }

                            break;
                    }
                }

                SetPaletteLookup(index, (byte)r, (byte)g, (byte)b);
            }
        }

        public void SetPaletteType()
        {
            byte borderColor = CC3BorderColor;

            SetGimeBorderColor(0);
            MakeCmpPalette(_modules.Config.GetPaletteType());
            SetGimeBorderColor(borderColor);
        }

        public void SetMonitorType(byte type)
        {
            byte borderColor = CC3BorderColor;

            SetGimeBorderColor(0);

            MonType = (type & 1) == 0 ? Define.FALSE : Define.TRUE;

            for (byte palIndex = 0; palIndex < 16; palIndex++)
            {
                SetMonitorTypePalettes(MonType, palIndex);
            }

            SetGimeBorderColor(borderColor);
        }

        public void InvalidateBorder()
        {
            BorderChange = 5;
        }

        public void MakeRgbPalette()
        {
            for (byte index = 0; index < 64; index++)
            {
                //colors->PaletteLookup8[1][index] = index | 128;

                //r = colors->ColorTable16Bit[(index & 32) >> 4 | (index & 4) >> 2];
                //g = colors->ColorTable16Bit[(index & 16) >> 3 | (index & 2) >> 1];
                //b = colors->ColorTable16Bit[(index & 8) >> 2 | (index & 1)];
                //colors->PaletteLookup16[1][index] = (r << 11) | (g << 6) | b;

                //32BIT
                byte r = _colors.ColorTable32Bit[(index & 32) >> 4 | (index & 4) >> 2];
                byte g = _colors.ColorTable32Bit[(index & 16) >> 3 | (index & 2) >> 1];
                byte b = _colors.ColorTable32Bit[(index & 8) >> 2 | (index & 1)];

                _colors.PaletteLookup32[1 * 64 + index] = (uint)(r << 16 | g << 8 | b);
            }
        }

        public void SetMonitorTypePalettes(byte monType, byte palIndex)
        {
            int offset = monType * 64 + palIndex;

            _colors.Palette16Bit[palIndex] = _colors.PaletteLookup16[offset]; //colors->PaletteLookup16[monType][colors->Palette[palIndex]];
            _colors.Palette32Bit[palIndex] = _colors.PaletteLookup32[offset]; //colors->PaletteLookup32[monType][colors->Palette[palIndex]];
            _colors.Palette8Bit[palIndex] = _colors.PaletteLookup8[offset]; //colors->PaletteLookup8[monType][colors->Palette[palIndex]];
        }

        public void SetGimeBorderColor(byte data)
        {
            if (CC3BorderColor != (data & 63))
            {
                CC3BorderColor = (byte)(data & 63);
                SetupDisplay();
                BorderChange = 3;
            }
        }

        public void SetPaletteLookup(byte index, byte r, byte g, byte b)
        {
            byte rr = r;
            byte gg = g;
            byte bb = b;

            _colors.PaletteLookup32[0 * 64 + index] = (uint)((rr << 16) | (gg << 8) | bb);

            //rr >>= 3;
            //gg >>= 3;
            //bb >>= 3;
            //colors->PaletteLookup16[0][index] = (rr << 11) | (gg << 6) | bb;

            //rr >>= 3;
            //gg >>= 3;
            //bb >>= 3;
            //colors->PaletteLookup8[0][index] = 0x80 | ((rr & 2) << 4) | ((gg & 2) << 3) | ((bb & 2) << 2) | ((rr & 1) << 2) | ((gg & 1) << 1) | (bb & 1);
        }

        public bool CheckState(byte attributes)
        {
            //return (!instance->BlinkState) & !!(attributes & 128);
            return (BlinkState == 0) && ((attributes & 128) != 0);
        }

        //3 bits from SAM Registers
        public void SetGimeVdgMode(byte vdgMode)
        {
            if (CC2VDGMode != vdgMode)
            {
                CC2VDGMode = vdgMode;
                SetupDisplay();
                BorderChange = 3;
            }
        }

        // These grab the Video info for all COCO 2 modes
        public void SetGimeVdgOffset(byte offset)
        {
            if (CC2Offset != offset)
            {
                CC2Offset = offset;
                SetupDisplay();
            }
        }

        public void SetGimeVmode(byte vmode)
        {
            if (CC3Vmode != vmode)
            {
                CC3Vmode = vmode;
                SetupDisplay();
                BorderChange = 3;
            }
        }

        public void SetGimeVres(byte vres)
        {
            if (CC3Vres != vres)
            {
                CC3Vres = vres;
                SetupDisplay();
                BorderChange = 3;
            }
        }

        //These grab the Video info for all COCO 3 modes
        public void SetVerticalOffsetRegister(ushort voRegister)
        {
            if (VerticalOffsetRegister != voRegister)
            {
                VerticalOffsetRegister = voRegister;

                SetupDisplay();
            }
        }

        public void SetGimeHorizontalOffset(byte data)
        {
            if (HorizontalOffsetReg != data)
            {
                HorizontalOffset = (byte)(data << 1);
                HorizontalOffsetReg = data;
                SetupDisplay();
            }
        }

        public void SetGimePalette(byte palette, byte color)
        {
            byte offset = (byte)(color & 63);
            int index = MonType * 64 + offset;

            // ReSharper disable CommentTypo
            // Convert the 6bit rgbrgb value to rrrrrggggggbbbbb for the Real video hardware.
            // ReSharper restore CommentTypo
            //	unsigned char r,g,b;
            _colors.Palette[palette] = offset;
            _colors.Palette8Bit[palette] = _colors.PaletteLookup8[index]; //colors->PaletteLookup8[instance->MonType][offset];
            _colors.Palette16Bit[palette] = _colors.PaletteLookup16[index]; //colors->PaletteLookup16[instance->MonType][offset];
            _colors.Palette32Bit[palette] = _colors.PaletteLookup32[index]; //colors->PaletteLookup32[instance->MonType][offset];
        }

        public void SetCompatMode(byte mode)
        {
            if (CompatMode != mode)
            {
                CompatMode = mode;
                SetupDisplay();
                BorderChange = 3;
            }
        }

        public void SetVideoBank(byte data)
        {
            DistoOffset = (uint)(data * (512 * 1024));

            SetupDisplay();
        }

        //5 bits from PIA Register
        public void SetGimeVdgMode2(byte mode)
        {
            if (CC2VDGPiaMode != mode)
            {
                CC2VDGPiaMode = mode;
                SetupDisplay();
                BorderChange = 3;
            }
        }

        public unsafe void SetGraphicsSurfaces(void* pSurface)
        {
            Surfaces.pSurface = pSurface;
        }

        public void SetupDisplay()
        {
            ExtendedText = 1;

            switch (CompatMode)
            {
                case 0:     //Color Computer 3 Mode
                    NewStartOfVidRam = (uint)(VerticalOffsetRegister * 8);
                    GraphicsMode = (byte)((CC3Vmode & 128) >> 7);
                    VresIndex = (byte)((CC3Vres & 96) >> 5);
                    CoCo3LinesPerRow[7] = LinesPerScreen;   // For 1 pixel high modes
                    Bpp = (byte)(CC3Vres & 3);
                    LinesPerRow = CoCo3LinesPerRow[CC3Vmode & 7];
                    BytesPerRow = CoCo3BytesPerRow[(CC3Vres & 28) >> 2];
                    PaletteIndex = 0;

                    if (GraphicsMode != 0)
                    {
                        if ((CC3Vres & 1) != 0)
                        {
                            ExtendedText = 2;
                        }

                        Bpp = 0;
                        BytesPerRow = CoCo3BytesPerTextRow[(CC3Vres & 28) >> 2];
                    }

                    break;

                case 1:
                    //Color Computer 2 Mode
                    CC3BorderColor = 0;   //Black for text modes
                    BorderChange = 3;
                    NewStartOfVidRam = (uint)((512 * CC2Offset) + (VerticalOffsetRegister & 0xE0FF) * 8);
                    GraphicsMode = (byte)((CC2VDGPiaMode & 16) >> 4); //PIA Set on graphics clear on text
                    VresIndex = 0;
                    LinesPerRow = CoCo2LinesPerRow[CC2VDGMode];

                    byte colorSet;
                    byte tmpByte;

                    if (GraphicsMode != 0)
                    {
                        colorSet = (byte)(CC2VDGPiaMode & 1);

                        CC3BorderColor = colorSet == 0 ? (byte)18 : (byte)63;

                        BorderChange = 3;
                        Bpp = CoCo2Bpp[(CC2VDGPiaMode & 15) >> 1];
                        BytesPerRow = CoCo2BytesPerRow[(CC2VDGPiaMode & 15) >> 1];
                        tmpByte = (byte)((CC2VDGPiaMode & 1) << 1 | (Bpp & 1));
                        PaletteIndex = CoCo2PaletteSet[tmpByte];
                    }
                    else
                    {   //Setup for 32x16 text Mode
                        Bpp = 0;
                        BytesPerRow = 32;
                        InvertAll = (byte)((CC2VDGPiaMode & 4) >> 2);
                        LowerCase = (byte)((CC2VDGPiaMode & 2) >> 1);
                        colorSet = (byte)(CC2VDGPiaMode & 1);
                        tmpByte = (byte)((colorSet << 1) | InvertAll);

                        switch (tmpByte)
                        {
                            case 0:
                                TextFgPalette = 12;
                                TextBgPalette = 13;
                                break;

                            case 1:
                                TextFgPalette = 13;
                                TextBgPalette = 12;
                                break;

                            case 2:
                                TextFgPalette = 14;
                                TextBgPalette = 15;
                                break;

                            case 3:
                                TextFgPalette = 15;
                                TextBgPalette = 14;
                                break;
                        }
                    }

                    break;
            }

            //gs->ColorInvert = (gs->CC3Vmode & 32) >> 5;
            LinesPerScreen = Lpf[VresIndex];

            _modules.CoCo.SetLinesPerScreen(VresIndex);

            VerticalCenter = (byte)(VcenterTable[VresIndex] - 4); //4 un-rendered top lines
            PixelsPerLine = (ushort)(BytesPerRow * PixelsPerByte[Bpp]);

            if ((PixelsPerLine % 40) != 0)
            {
                Stretch = (byte)((512 / PixelsPerLine) - 1);
                HorizontalCenter = 64;
            }
            else
            {
                Stretch = (byte)((640 / PixelsPerLine) - 1);
                HorizontalCenter = 0;
            }

            VPitch = BytesPerRow;

            if ((HorizontalOffsetReg & 128) != 0)
            {
                VPitch = 256;
            }

            byte offset = (byte)(CC3BorderColor & 63);
            int index = MonType * 64 + offset;

            BorderColor8 = (byte)(offset | 128);
            BorderColor16 = _colors.PaletteLookup16[index]; //colors->PaletteLookup16[instance->MonType][instance->CC3BorderColor & 63];
            BorderColor32 = _colors.PaletteLookup32[index]; //colors->PaletteLookup32[instance->MonType][instance->CC3BorderColor & 63];

            NewStartOfVidRam = (NewStartOfVidRam & VidMask) + DistoOffset; //Dist Offset for 2M configuration
            MasterMode = (byte)((GraphicsMode << 7) | (CompatMode << 6) | ((Bpp & 3) << 4) | (Stretch & 15));
        }
    }
}
