using System.Diagnostics;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Models;
using VCCSharp.Models.Configuration;
using VCCSharp.Models.Graphics;
using VCCSharp.Modules.TCC1014;
using VCCSharp.Modules.TCC1014.Masks;

namespace VCCSharp.Modules;

public interface IGraphics : IModule
{
    GraphicsColors GetGraphicsColors();

    IntPointer GetGraphicsSurface();

    void ResetGraphicsState();
    void SetBlinkState(byte state);
    void SetBorderChange();
    void SetVidMask(MemorySizes memorySize);
    void SetPaletteType();
    void SetMonitorType();
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
    void SetCompatMode(CompatibilityModes mode);
    void SetVideoBank(byte data);
    void SetGimeVdgMode2(byte mode);
    void SetGraphicsSurface(IntPtr surface);
    bool InTextMode();

    byte BorderChange { get; set; }
    byte BytesPerRow { get; set; }
    bool ColorInvert { get; set; }
    byte ExtendedText { get; set; }
    byte GraphicsMode { get; set; }
    byte HorizontalOffset { get; set; }
    byte HorizontalCenter { get; set; }
    byte HorizontalOffsetReg { get; set; }
    byte LinesPerRow { get; set; }
    byte LinesPerScreen { get; set; }
    bool LowerCase { get; set; }
    byte MasterMode { get; set; }
    MonitorTypes MonitorType { get; set; }
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
    uint BorderColor { get; set; }

    byte[] Lpf { get; }
    byte[] VerticalCenterTable { get; }
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

#pragma warning disable IDE1006 // Naming Styles
    private static readonly byte[] CoCo2Bpp = { 1, 0, 1, 0, 1, 0, 1, 0 };
    private static readonly byte[] CoCo2LinesPerRow = { 12, 3, 3, 2, 2, 1, 1, 1 };
    private static readonly byte[] CoCo3LinesPerRow = { 1, 1, 2, 8, 9, 10, 11, 200 };
    private static readonly byte[] CoCo2BytesPerRow = { 16, 16, 32, 16, 32, 16, 32, 32 };
    private static readonly byte[] CoCo3BytesPerRow = { 16, 20, 32, 40, 64, 80, 128, 160 };
    private static readonly byte[] CoCo3BytesPerTextRow = { 32, 40, 32, 40, 64, 80, 64, 80 };
    private static readonly byte[] CoCo2PaletteSet = { 8, 0, 10, 4 };
    private static readonly byte[] PixelsPerByte = { 8, 4, 2, 2 };
#pragma warning restore IDE1006 // Naming Styles

    private readonly IModules _modules;
    private readonly IConfiguration _configuration;

    private readonly GraphicsColors _colors = new();
    private readonly VideoMasks _videoMask = new();

    public byte BlinkState { get; set; }

    public byte BorderChange { get; set; } = 3;
    public byte Bpp { get; set; }
    public byte BytesPerRow { get; set; } = 32;
    public byte CC2Offset { get; set; }
    public byte CC2VdgMode { get; set; }
    public byte CC2VdgPiaMode { get; set; }
    public byte CC3BorderColor { get; set; }
    public byte CC3Vmode { get; set; }
    public byte CC3Vres { get; set; }
    public bool ColorInvert { get; set; } = true;
    public CompatibilityModes CompatibilityMode { get; set; }
    public byte ExtendedText { get; set; } = 1;
    public byte GraphicsMode { get; set; }
    public byte HorizontalOffset { get; set; }
    public byte HorizontalCenter { get; set; }
    public byte HorizontalOffsetReg { get; set; }
    public byte InvertAll { get; set; }
    public byte LinesPerRow { get; set; } = 1;
    public byte LinesPerScreen { get; set; }
    public bool LowerCase { get; set; }
    public byte MasterMode { get; set; }
    public MonitorTypes MonitorType { get; set; } = MonitorTypes.RGB;
    public byte PaletteIndex { get; set; }
    public byte Stretch { get; set; }
    public byte TextBgPalette { get; set; }
    public byte TextFgPalette { get; set; }
    public byte VerticalCenter { get; set; }
    public byte VresIndex { get; set; }

    public ushort PixelsPerLine { get; set; }
    public ushort TagY { get; set; }
    public ushort VerticalOffsetRegister { get; set; }
    public ushort VPitch { get; set; } = 32;

    public uint VidRamOffset { get; set; }
    public uint NewStartOfVidRam { get; set; }
    public uint StartOfVidRam { get; set; }
    public uint VidMask { get; set; } = 0x1FFFF;

    public uint BorderColor { get; set; }

    //--Original CoCo had 192 fixed drawable lines.  Looks like CoCo 3 can do 225.
    public byte[] Lpf { get; } = { 192, 225 };
    public byte[] VerticalCenterTable { get; } = { 29, 12 };

    private IntPointer? _surface;

    public Graphics(IModules modules, IConfiguration configuration)
    {
        _modules = modules;
        _configuration = configuration;
    }

    public IntPointer GetGraphicsSurface()
    {
        if (_surface == null) throw new Exception("Unable to get graphics surface");
        return _surface;
    }

    public GraphicsColors GetGraphicsColors() => _colors;

    public void ResetGraphicsState()
    {
        CC3Vmode = 0;
        CC3Vres = 0;
        StartOfVidRam = 0;
        NewStartOfVidRam = 0;
        GraphicsMode = 0;
        LowerCase = false;
        InvertAll = 0;
        ExtendedText = 1;
        HorizontalOffsetReg = 0;
        TagY = 0;
        VidRamOffset = 0;
        BorderChange = 3;
        CC2Offset = 0;
        HorizontalOffset = 0;
        VerticalOffsetRegister = 0;

        _colors.Palette32Bit.Reset();
    }

    public void ResetPalette()
    {
        MakeRgbPalette();
        MakeCmpPalette(_modules.Configuration.Video.Palette.Value);
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

    public void SetVidMask(MemorySizes memorySize)
    {
        VidMask = _videoMask[memorySize];
    }

    public void FlipArtifacts()
    {
        ColorInvert = !ColorInvert;
    }

    //TODO: ScanLines never really worked right to begin with...
    public void SetScanLines()
    {
        _modules.Emu.ScanLines = _configuration.Video.ScanLines;
        _modules.Emu.ResetPending = ResetPendingStates.Cls;

        _modules.Draw.ClearScreen();

        BorderChange = 3;
    }

    public void MakeCmpPalette(PaletteTypes paletteType)
    {
        Debug.WriteLine(paletteType == PaletteTypes.Original ? "Loading original CMP palette." : "Loading updated CMP palette.");

        float gamma = 1.4f;

        for (byte index = 0; index <= 63; index++)
        {
            double r, g, b;

            if (paletteType == PaletteTypes.Original)
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

            _colors.PaletteLookup32[MonitorTypes.Composite][index] = (uint)(((byte)r << 16) | ((byte)g << 8) | (byte)b);
        }
    }

    public void SetPaletteType()
    {
        byte borderColor = CC3BorderColor;

        SetGimeBorderColor(0);

        MakeRgbPalette();
        MakeCmpPalette(_modules.Configuration.Video.Palette.Value);

        SetGimeBorderColor(borderColor);
    }

    public void SetMonitorType()
    {
        MonitorType = _configuration.Video.Monitor.Value;

        Debug.WriteLine($"Monitor Type={MonitorType}");

        byte borderColor = CC3BorderColor;

        SetGimeBorderColor(0);

        _colors.Palette32Bit.Map(_colors.PaletteLookup32[MonitorType]);

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
            //32BIT
            byte r = _colors.ColorTable32Bit[(index & 32) >> 4 | (index & 4) >> 2];
            byte g = _colors.ColorTable32Bit[(index & 16) >> 3 | (index & 2) >> 1];
            byte b = _colors.ColorTable32Bit[(index & 8) >> 2 | (index & 1)];

            _colors.PaletteLookup32[MonitorTypes.RGB][index] = (uint)(r << 16 | g << 8 | b);
        }
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

    public bool CheckState(byte attributes)
    {
        return BlinkState == 0 && (attributes & 128) != 0;
    }

    //3 bits from SAM Registers
    public void SetGimeVdgMode(byte vdgMode)
    {
        if (CC2VdgMode != vdgMode)
        {
            CC2VdgMode = vdgMode;
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

        // ReSharper disable CommentTypo
        // Convert the 6bit rgbrgb value to rrrrrggggggbbbbb for the Real video hardware.
        // ReSharper restore CommentTypo
        _colors.Palette[palette] = offset;

        //--This is swapping out palette indexes
        _colors.Palette32Bit.Map(_colors.PaletteLookup32[MonitorType], palette, offset);
    }

    public void SetCompatMode(CompatibilityModes mode)
    {
        if (CompatibilityMode != mode)
        {
            CompatibilityMode = mode;
            SetupDisplay();
            BorderChange = 3;
        }
    }

    public void SetVideoBank(byte data)
    {
        VidRamOffset = (uint)(data * (512 * 1024));

        SetupDisplay();
    }

    //5 bits from PIA Register
    public void SetGimeVdgMode2(byte mode)
    {
        if (CC2VdgPiaMode != mode)
        {
            CC2VdgPiaMode = mode;
            SetupDisplay();
            BorderChange = 3;
        }
    }

    public void SetGraphicsSurface(IntPtr surface)
    {
        _surface = new IntPointer(surface);
    }

    public void SetupDisplay()
    {
        ExtendedText = 1;

        switch (CompatibilityMode)
        {
            //Color Computer 3 Mode
            case CompatibilityModes.CoCo3:
                NewStartOfVidRam = (uint)(VerticalOffsetRegister * 8);
                GraphicsMode = (byte)((CC3Vmode & 128) >> 7);
                VresIndex = (CC3Vres & 96) == 0 ? Define._192Lines : Define._225Lines;
                LinesPerRow = CoCo3LinesPerRow[CC3Vmode & 7];

                Bpp = (byte)(CC3Vres & 3);
                CoCo3LinesPerRow[7] = LinesPerScreen;   // For 1 pixel high modes
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

            //Color Computer 2 Mode
            case CompatibilityModes.CoCo2:
                NewStartOfVidRam = (uint)(512 * CC2Offset + (VerticalOffsetRegister & 0xE0FF) * 8);
                GraphicsMode = (byte)((CC2VdgPiaMode & 16) >> 4); //PIA Set on graphics clear on text
                VresIndex = Define._192Lines;
                LinesPerRow = CoCo2LinesPerRow[CC2VdgMode];

                CC3BorderColor = 0;   //Black for text modes
                BorderChange = 3;

                byte colorSet;
                byte tmpByte;

                if (GraphicsMode != 0)
                {
                    colorSet = (byte)(CC2VdgPiaMode & 1);

                    CC3BorderColor = colorSet == 0 ? (byte)18 : (byte)63;

                    BorderChange = 3;
                    Bpp = CoCo2Bpp[(CC2VdgPiaMode & 15) >> 1];
                    BytesPerRow = CoCo2BytesPerRow[(CC2VdgPiaMode & 15) >> 1];
                    tmpByte = (byte)((CC2VdgPiaMode & 1) << 1 | (Bpp & 1));
                    PaletteIndex = CoCo2PaletteSet[tmpByte];
                }
                else
                {   //Setup for 32x16 text Mode
                    Bpp = 0;
                    BytesPerRow = 32;
                    InvertAll = (byte)((CC2VdgPiaMode & 4) >> 2);
                    LowerCase = (CC2VdgPiaMode & 2) >> 1 != 0;
                    colorSet = (byte)(CC2VdgPiaMode & 1);
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

        VerticalCenter = (byte)(VerticalCenterTable[VresIndex] - 4); //4 un-rendered top lines
        PixelsPerLine = (ushort)(BytesPerRow * PixelsPerByte[Bpp]);

        if (PixelsPerLine % 40 != 0)
        {
            Stretch = (byte)(512 / PixelsPerLine - 1);
            //TODO: Figure out this magic number
            HorizontalCenter = 80; //64;
        }
        else
        {
            Stretch = (byte)(640 / PixelsPerLine - 1);
            HorizontalCenter = 0;
        }

        VPitch = BytesPerRow;

        if ((HorizontalOffsetReg & 128) != 0)
        {
            VPitch = 256;
        }

        byte offset = (byte)(CC3BorderColor & 63);

        BorderColor = _colors.PaletteLookup32[MonitorType][offset];

        NewStartOfVidRam = (NewStartOfVidRam & VidMask) + VidRamOffset; //Dist Offset for 2M configuration
        MasterMode = (byte)((GraphicsMode << 7) | ((int)CompatibilityMode << 6) | ((Bpp & 3) << 4) | (Stretch & 15));
    }

    public bool InTextMode()
    {
        int graphicsMode = GraphicsMode;

        if (graphicsMode != 0)
        {
            const string warning = "Warning: You are not in text mode. Continue Pasting?";

            var result = MessageBox.Show(warning, "Clipboard", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
            {
                return false;
            }
        }

        return true;
    }

    public void ModuleReset()
    {
        SetMonitorType();
        SetPaletteType();
        SetScanLines();
    }
}
