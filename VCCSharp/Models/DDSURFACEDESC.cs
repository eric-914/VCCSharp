using System;
using System.Runtime.InteropServices;

namespace VCCSharp.Models
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

    }
}
