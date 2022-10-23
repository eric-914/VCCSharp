#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable once InconsistentNaming

namespace VCCSharp.Modules.TCC1014.Modes;

public static class _128_47
{
    //Bpp=2 Sr=15 4BPP Stretch=16
    public static void Mode(ModeModel model, int start, int yStride)
    {
        IGraphics graphics = model.Modules.Graphics;
        IEmu emu = model.Modules.Emu;

        var palette = graphics.GetGraphicsColors().Palette32Bit;
        var szSurface32 = graphics.GetGraphicsSurface();

        int xPitch = (int)emu.SurfacePitch;
        var memory = model.ShortPointer;

        for (ushort beam = 0; beam < graphics.BytesPerRow; beam += 2) //4bbp Stretch=16
        {
            long index = (graphics.VidMask & (start + (byte)(graphics.HorizontalOffset + beam))) >> 1;
            ushort widePixel = memory[index];

            szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
            szSurface32[yStride += 1] = palette[15 & widePixel];
            szSurface32[yStride += 1] = palette[15 & widePixel];
            szSurface32[yStride += 1] = palette[15 & widePixel];
            szSurface32[yStride += 1] = palette[15 & widePixel];
            szSurface32[yStride += 1] = palette[15 & widePixel];
            szSurface32[yStride += 1] = palette[15 & widePixel];
            szSurface32[yStride += 1] = palette[15 & widePixel];
            szSurface32[yStride += 1] = palette[15 & widePixel];
            szSurface32[yStride += 1] = palette[15 & widePixel];
            szSurface32[yStride += 1] = palette[15 & widePixel];
            szSurface32[yStride += 1] = palette[15 & widePixel];
            szSurface32[yStride += 1] = palette[15 & widePixel];
            szSurface32[yStride += 1] = palette[15 & widePixel];
            szSurface32[yStride += 1] = palette[15 & widePixel];
            szSurface32[yStride += 1] = palette[15 & widePixel];
            szSurface32[yStride += 1] = palette[15 & widePixel];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
            szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];

            if (!emu.ScanLines)
            {
                yStride -= (64);
                yStride += xPitch;
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[15 & widePixel];
                szSurface32[yStride += 1] = palette[15 & widePixel];
                szSurface32[yStride += 1] = palette[15 & widePixel];
                szSurface32[yStride += 1] = palette[15 & widePixel];
                szSurface32[yStride += 1] = palette[15 & widePixel];
                szSurface32[yStride += 1] = palette[15 & widePixel];
                szSurface32[yStride += 1] = palette[15 & widePixel];
                szSurface32[yStride += 1] = palette[15 & widePixel];
                szSurface32[yStride += 1] = palette[15 & widePixel];
                szSurface32[yStride += 1] = palette[15 & widePixel];
                szSurface32[yStride += 1] = palette[15 & widePixel];
                szSurface32[yStride += 1] = palette[15 & widePixel];
                szSurface32[yStride += 1] = palette[15 & widePixel];
                szSurface32[yStride += 1] = palette[15 & widePixel];
                szSurface32[yStride += 1] = palette[15 & widePixel];
                szSurface32[yStride += 1] = palette[15 & widePixel];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[15 & (widePixel >> 8)];
                yStride -= xPitch;
            }
        }
    }
}