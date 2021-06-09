using VCCSharp.Modules.TC1014.Modes;

// ReSharper disable once CheckNamespace
namespace VCCSharp.Modules.TC1014
{
    // ReSharper disable once InconsistentNaming
    public partial class TC1014
    {
        //Bpp=1 Sr=7  2BPP Stretch=8
        //Bpp=1 Sr=8
        //Bpp=1 Sr=9 
        //Bpp=1 Sr=10 
        //Bpp=1 Sr=11 
        //Bpp=1 Sr=12 
        //Bpp=1 Sr=13 
        //Bpp=1 Sr=14
        public unsafe void Mode128_23_30(ModeModel model, int start, int yStride)
        {
            uint[] palette = model.Modules.Graphics.GetGraphicsColors().Palette32Bit;
            uint* szSurface32 = model.Modules.Graphics.GetGraphicsSurfaces().pSurface32;
            ushort* wMemory = (ushort*) model.Memory;
            int xPitch = (int)model.Modules.Emu.SurfacePitch;

            for (ushort beam = 0; beam < Graphics.BytesPerRow; beam += 2) //2bbp Stretch=8
            {
                uint widePixel = wMemory[(Graphics.VidMask & (start + (byte)(Graphics.HorizontalOffset + beam))) >> 1];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                szSurface32[yStride += 1] = palette[3 & widePixel];
                szSurface32[yStride += 1] = palette[3 & widePixel];
                szSurface32[yStride += 1] = palette[3 & widePixel];
                szSurface32[yStride += 1] = palette[3 & widePixel];
                szSurface32[yStride += 1] = palette[3 & widePixel];
                szSurface32[yStride += 1] = palette[3 & widePixel];
                szSurface32[yStride += 1] = palette[3 & widePixel];
                szSurface32[yStride += 1] = palette[3 & widePixel];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];

                if (!_modules.Emu.ScanLines)
                {
                    yStride -= (64);
                    yStride += xPitch;
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 6)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 4)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 2)];
                    szSurface32[yStride += 1] = palette[3 & widePixel];
                    szSurface32[yStride += 1] = palette[3 & widePixel];
                    szSurface32[yStride += 1] = palette[3 & widePixel];
                    szSurface32[yStride += 1] = palette[3 & widePixel];
                    szSurface32[yStride += 1] = palette[3 & widePixel];
                    szSurface32[yStride += 1] = palette[3 & widePixel];
                    szSurface32[yStride += 1] = palette[3 & widePixel];
                    szSurface32[yStride += 1] = palette[3 & widePixel];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 14)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 12)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 10)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                    szSurface32[yStride += 1] = palette[3 & (widePixel >> 8)];
                    yStride -= xPitch;
                }
            }
        }
    }
}
