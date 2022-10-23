#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable once InconsistentNaming

namespace VCCSharp.Modules.TCC1014.Modes;

public static class _128_39_46
{
    //Bpp=2 Sr=7 4BPP Stretch=8
    //Bpp=2 Sr=8 
    //Bpp=2 Sr=9 
    //Bpp=2 Sr=10 
    //Bpp=2 Sr=11 
    //Bpp=2 Sr=12 
    //Bpp=2 Sr=13 
    //Bpp=2 Sr=14 
    public static void Mode(ModeModel model, int start, int yStride)
    {
        IGraphics graphics = model.Modules.Graphics;
        IEmu emu = model.Modules.Emu;

        var palette = graphics.GetGraphicsColors().Palette32Bit;
        var szSurface32 = graphics.GetGraphicsSurface();

        int xPitch = (int)emu.SurfacePitch;
        var memory = model.ShortPointer;

        for (ushort beam = 0; beam < graphics.BytesPerRow; beam += 2) //4bbp Stretch=8
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
                yStride -= (32);
                yStride += xPitch;
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
                yStride -= xPitch;
            }
        }
    }
}