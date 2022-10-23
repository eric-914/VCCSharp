#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable once InconsistentNaming

namespace VCCSharp.Modules.TCC1014.Modes;

public static class _192_19_22
{
    //Bpp=1 Sr=3  2BPP Stretch=4
    //Bpp=1 Sr=4
    //Bpp=1 Sr=5
    //Bpp=1 Sr=6
    public static void Mode(ModeModel model, int start, int yStride)
    {
        IGraphics graphics = model.Modules.Graphics;
        IEmu emu = model.Modules.Emu;

        var palette = graphics.GetGraphicsColors().Palette32Bit;
        var szSurface32 = graphics.GetGraphicsSurface();

        int xPitch = (int)emu.SurfacePitch;
        var memory = model.ShortPointer;

        for (ushort beam = 0; beam < graphics.BytesPerRow; beam += 2) //2bbp Stretch=4
        {
            long index = (graphics.VidMask & (start + (byte)(graphics.HorizontalOffset + beam))) >> 1;
            ushort widePixel = memory[index];

            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 6))];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 6))];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 6))];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 6))];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 4))];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 4))];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 4))];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 4))];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 2))];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 2))];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 2))];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 2))];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & widePixel)];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & widePixel)];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & widePixel)];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & widePixel)];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 14))];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 14))];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 14))];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 14))];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 12))];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 12))];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 12))];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 12))];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 10))];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 10))];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 10))];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 10))];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 8))];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 8))];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 8))];
            szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 8))];

            if (!emu.ScanLines)
            {
                yStride -= (32);
                yStride += xPitch;
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 6))];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 6))];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 6))];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 6))];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 4))];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 4))];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 4))];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 4))];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 2))];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 2))];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 2))];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 2))];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & widePixel)];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & widePixel)];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & widePixel)];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & widePixel)];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 14))];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 14))];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 14))];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 14))];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 12))];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 12))];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 12))];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 12))];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 10))];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 10))];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 10))];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 10))];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 8))];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 8))];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 8))];
                szSurface32[yStride += 1] = palette[graphics.PaletteIndex + (3 & (widePixel >> 8))];
                yStride -= xPitch;
            }
        }
    }
}