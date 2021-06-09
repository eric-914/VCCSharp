using VCCSharp.Modules.TC1014.Modes;

// ReSharper disable once CheckNamespace
namespace VCCSharp.Modules.TC1014
{
    // ReSharper disable once InconsistentNaming
    public partial class TC1014
    {
        //Bpp=2 Sr=15 4BPP Stretch=16
        public unsafe void Mode128_47(ModeModel model, int start, int yStride)
        {
            uint[] palette = model.Modules.Graphics.GetGraphicsColors().Palette32Bit;
            uint* szSurface32 = model.Modules.Graphics.GetGraphicsSurfaces().pSurface32;
            ushort* wMemory = (ushort*) model.Memory;
            int xPitch = (int)model.Modules.Emu.SurfacePitch;

            for (ushort beam = 0; beam < Graphics.BytesPerRow; beam += 2) //4bbp Stretch=16
            {
                uint widePixel = wMemory[(Graphics.VidMask & (start + (byte)(Graphics.HorizontalOffset + beam))) >> 1];
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

                if (!_modules.Emu.ScanLines)
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
}
