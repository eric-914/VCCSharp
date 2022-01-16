using System;
using System.Runtime.InteropServices;

namespace VCCSharp.DX8.Models
{
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once IdentifierTypo
    [StructLayout(LayoutKind.Explicit, Size = 120, CharSet = CharSet.Ansi)]
    public struct DDSURFACEDESC
    {
        [FieldOffset(0)]
        public uint dwSize;

        [FieldOffset(4)]
        public uint dwFlags;                // determines what fields are valid

        [FieldOffset(8)]
        public uint dwHeight;               // height of surface to be created

        [FieldOffset(12)]
        public uint dwWidth;                // width of input surface

        #region union
        [FieldOffset(16)]
        public long lPitch;                 // distance to start of next line (return value only)

        [FieldOffset(16)]
        public uint dwLinearSize;           // Formless late-allocated optimized surface size
        #endregion

        [FieldOffset(24)]
        public uint dwBackBufferCount;      // number of back buffers requested

        #region union
        [FieldOffset(28)]
        public uint dwMipMapCount;      // number of back buffers requested

        [FieldOffset(28)]
        public uint dwZBufferBitDepth;  // depth of Z buffer requested

        [FieldOffset(28)]
        public uint dwRefreshRate;      // refresh rate (used when display mode is described)
        #endregion

        [FieldOffset(32)]
        public uint dwAlphaBitDepth;        // depth of alpha buffer requested

        [FieldOffset(36)]
        public uint dwReserved;             // reserved

        [FieldOffset(40)]
        public IntPtr lpSurface;      // pointer to the associated surface memory

        [FieldOffset(48)]
        public DDCOLORKEY ddckCKDestOverlay;

        [FieldOffset(56)]
        public DDCOLORKEY ddckCKDestBlt;

        [FieldOffset(64)]
        public DDCOLORKEY ddckCKSrcOverlay;

        [FieldOffset(72)]
        public DDCOLORKEY ddckCKSrcBlt;

        [FieldOffset(80)]
        public DDPIXELFORMAT ddpfPixelFormat;

        [FieldOffset(112)]
        public DDSCAPS ddsCaps;
    }

    [StructLayout(LayoutKind.Explicit, Size = 8, CharSet = CharSet.Ansi)]
    public struct DDCOLORKEY
    {
        [FieldOffset(0)]
        public uint dwColorSpaceLowValue;

        [FieldOffset(4)]
        public uint dwColorSpaceHighValue;
    };

    [StructLayout(LayoutKind.Explicit, Size = 32, CharSet = CharSet.Ansi)]
    public struct DDPIXELFORMAT
    {
        [FieldOffset(0)]
        public uint dwSize;

        [FieldOffset(4)]
        public uint dwFlags;

        [FieldOffset(8)]
        public uint dwFourCC;

        #region union
        [FieldOffset(12)]
        public uint dwRGBBitCount;          // how many bits per pixel

        [FieldOffset(12)]
        public uint dwYUVBitCount;          // how many bits per pixel

        [FieldOffset(12)]
        public uint dwZBufferBitDepth;      // how many total bits/pixel in z buffer (including any stencil bits)

        [FieldOffset(12)]
        public uint dwAlphaBitDepth;        // how many bits for alpha channels

        [FieldOffset(12)]
        public uint dwLuminanceBitCount;    // how many bits per pixel

        [FieldOffset(12)]
        public uint dwBumpBitCount;         // how many bits per "buxel", total

        [FieldOffset(12)]
        public uint dwPrivateFormatBitCount;// Bits per pixel of private driver formats. Only valid in texture
        #endregion

        #region union
        [FieldOffset(16)]
        public uint dwRBitMask;             // mask for red bit

        [FieldOffset(16)]
        public uint dwYBitMask;             // mask for Y bits

        [FieldOffset(16)]
        public uint dwStencilBitDepth;      // how many stencil bits (note: dwZBufferBitDepth-dwStencilBitDepth is total Z-only bits)

        [FieldOffset(16)]
        public uint dwLuminanceBitMask;     // mask for luminance bits

        [FieldOffset(16)]
        public uint dwBumpDuBitMask;        // mask for bump map U delta bits

        [FieldOffset(16)]
        public uint dwOperations;           // DDPF_D3DFORMAT Operations
        #endregion

        #region union
        [FieldOffset(20)]
        public uint dwGBitMask;             // mask for green bits

        [FieldOffset(20)]
        public uint dwUBitMask;             // mask for U bits

        [FieldOffset(20)]
        public uint dwZBitMask;             // mask for Z bits

        [FieldOffset(20)]
        public uint dwBumpDvBitMask;        // mask for bump map V delta bits

        [FieldOffset(20)]
        public MultiSampleCaps caps;
        #endregion

        #region union
        [FieldOffset(24)]
        public uint dwBBitMask;             // mask for blue bits

        [FieldOffset(24)]
        public uint dwVBitMask;             // mask for V bits
        
        [FieldOffset(24)]
        public uint dwStencilBitMask;       // mask for stencil bits

        [FieldOffset(24)]
        public uint dwBumpLuminanceBitMask; // mask for luminance in bump map
        #endregion

        #region union
        [FieldOffset(28)]
        public uint dwRGBAlphaBitMask;      // mask for alpha channel

        [FieldOffset(28)]
        public uint dwYUVAlphaBitMask;      // mask for alpha channel

        [FieldOffset(28)]
        public uint dwLuminanceAlphaBitMask;// mask for alpha channel

        [FieldOffset(28)]
        public uint dwRGBZBitMask;          // mask for Z channel

        [FieldOffset(28)]
        public uint dwYUVZBitMask;          // mask for Z channel
        #endregion
    }

    public struct MultiSampleCaps
    {
        public ushort wFlipMSTypes;     // Multisample methods supported via flip for this D3DFORMAT
        public ushort wBltMSTypes;      // Multisample methods supported via blt for this D3DFORMAT
    }
}
