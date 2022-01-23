// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo

using System;
using System.Runtime.InteropServices;

namespace DX8.Internal.Models
{
    /// <summary>
    /// The DDSURFACEDESC structure contains a description of a surface to be created by the driver.
    /// </summary>
    /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/hardware/drivers/ff550339(v=vs.85)"/>
    [StructLayout(LayoutKind.Explicit, Size = MEMBLOCK.DDSURFACEDESC, CharSet = CharSet.Ansi)]
    internal struct DDSURFACEDESC
    {
        /// <summary>
        /// Specifies the size in bytes of this DDSURFACEDESC structure. This member must be initialized before the structure is used.
        /// </summary>
        [FieldOffset(0)]
        public uint dwSize;

        /// <summary>
        /// Specifies a set of flags that determine what members of the DDSURFACEDESC structure contain valid data. 
        /// </summary>
        [FieldOffset(4)]
        public uint dwFlags;                // determines what fields are valid

        /// <summary>
        /// Specifies the height of surface, in pixels.
        /// </summary>
        [FieldOffset(8)]
        public uint dwHeight;               // height of surface to be created

        /// <summary>
        /// Specifies the width of the surface, in pixels.
        /// </summary>
        [FieldOffset(12)]
        public uint dwWidth;                // width of input surface

        #region union

        /// <summary>
        /// Specifies the number of bytes between the beginnings of two adjacent scan lines; that is, the number of bytes to add to the beginning address of one scan line to reach the beginning address of the next scan line below it. The driver's DdCreateSurface callback must return this value.
        /// </summary>
        [FieldOffset(16)]
        public long lPitch;                 // distance to start of next line (return value only)

        /// <summary>
        /// Specifies the size in bytes of a formless, late-allocated, optimized surface.
        /// </summary>
        [FieldOffset(16)]
        public uint dwLinearSize;           // Formless late-allocated optimized surface size

        #endregion

        /// <summary>
        /// Specifies the number of back buffers associated with the surface.
        /// </summary>
        [FieldOffset(24)]
        public uint dwBackBufferCount;      // number of back buffers requested

        #region union

        /// <summary>
        /// Specifies the number of mipmap levels.
        /// </summary>
        [FieldOffset(28)]
        public uint dwMipMapCount;      // number of back buffers requested

        /// <summary>
        /// Specifies the depth of z-buffer in bits per pixel.
        /// </summary>
        [FieldOffset(28)]
        public uint dwZBufferBitDepth;  // depth of Z buffer requested

        /// <summary>
        /// Specifies the refresh rate in hertz of the monitor (used when the display mode is described).
        /// </summary>
        [FieldOffset(28)]
        public uint dwRefreshRate;      // refresh rate (used when display mode is described)

        #endregion

        /// <summary>
        /// Specifies the depth of the alpha buffer in bits per pixel.
        /// </summary>
        [FieldOffset(32)]
        public uint dwAlphaBitDepth;        // depth of alpha buffer requested

        /// <summary>
        /// Reserved and should be set to zero.
        /// </summary>
        [FieldOffset(36)]
        public uint dwReserved;             // reserved

        /// <summary>
        /// Specifies the address of the associated surface memory.
        /// </summary>
        [FieldOffset(40)]
        public IntPtr lpSurface;      // pointer to the associated surface memory

        /// <summary>
        /// Specifies the color key for destination overlay use.
        /// </summary>
        [FieldOffset(48)]
        public DDCOLORKEY ddckCKDestOverlay;

        /// <summary>
        /// Specifies the color key for destination blt use.
        /// </summary>
        [FieldOffset(56)]
        public DDCOLORKEY ddckCKDestBlt;

        /// <summary>
        /// Specifies the color key for source overlay use.
        /// </summary>
        [FieldOffset(64)]
        public DDCOLORKEY ddckCKSrcOverlay;

        /// <summary>
        /// Specifies the color key for source blt use.
        /// </summary>
        [FieldOffset(72)]
        public DDCOLORKEY ddckCKSrcBlt;

        /// <summary>
        /// Specifies a DDPIXELFORMAT structure that describes the pixel format of the surface.
        /// </summary>
        /// <see href="https://msdn.microsoft.com/library/ff550274(v=vs.85)"/>
        [FieldOffset(80)]
        public DDPIXELFORMAT ddpfPixelFormat;

        /// <summary>
        /// Specifies a DDSCAPS structure that contains the Microsoft DirectDrawMicrosoft surface capabilities.
        /// </summary>
        [FieldOffset(112)]
        public DDSCAPS ddsCaps;

        public static int Size => MEMBLOCK.DDSURFACEDESC;
    }

    /// <summary>
    /// The DDCOLORKEY structure describes a source color key, destination color key, or color space. A color key is specified if the low and high range values are the same. This structure is used with the GetColorKey and SetColorKey methods.
    /// </summary>
    /// <see href="https://docs.microsoft.com/en-us/windows/win32/api/ddraw/ns-ddraw-ddcolorkey"/>
    [StructLayout(LayoutKind.Explicit, Size = 8, CharSet = CharSet.Ansi)]
    internal struct DDCOLORKEY
    {
        /// <summary>
        /// Low value of the color range that is to be used as the color key.
        /// </summary>
        [FieldOffset(0)]
        public uint dwColorSpaceLowValue;

        /// <summary>
        /// High value of the color range that is to be used as the color key.
        /// </summary>
        [FieldOffset(4)]
        public uint dwColorSpaceHighValue;
    };

    /// <summary>
    /// The DDPIXELFORMAT structure describes the pixel format of a DirectDrawSurface object for the GetPixelFormat method.
    /// </summary>
    /// <see href="https://docs.microsoft.com/en-us/windows/win32/api/ddraw/ns-ddraw-ddpixelformat"/>
    [StructLayout(LayoutKind.Explicit, Size = 32, CharSet = CharSet.Ansi)]
    internal struct DDPIXELFORMAT
    {
        /// <summary>
        /// Size of the structure, in bytes. This member must be initialized before the structure is used.
        /// </summary>
        [FieldOffset(0)]
        public uint dwSize;

        /// <summary>
        /// Describe optional controls for this structure.
        /// </summary>
        [FieldOffset(4)]
        public uint dwFlags;

        /// <summary>
        /// A FourCC code.
        /// </summary>
        [FieldOffset(8)]
        public uint dwFourCC;

        #region union

        /// <summary>
        /// RGB bits per pixel (4, 8, 16, 24, or 32).
        /// </summary>
        [FieldOffset(12)]
        public uint dwRGBBitCount;          // how many bits per pixel

        /// <summary>
        /// YUV bits per pixel (4, 8, 16, 24, or 32).
        /// </summary>
        [FieldOffset(12)]
        public uint dwYUVBitCount;          // how many bits per pixel

        /// <summary>
        /// Z-buffer bit depth (8, 16, 24, or 32).
        /// </summary>
        [FieldOffset(12)]
        public uint dwZBufferBitDepth;      // how many total bits/pixel in z buffer (including any stencil bits)

        /// <summary>
        /// Alpha channel bit depth (1, 2, 4, or 8) for an alpha-only surface (DDPF_ALPHA). For pixel formats that contain alpha information interleaved with color data (DDPF_ALPHAPIXELS), count the bits in the dwRGBAlphaBitMask member to obtain the bit depth of the alpha component.
        /// </summary>
        [FieldOffset(12)]
        public uint dwAlphaBitDepth;        // how many bits for alpha channels

        /// <summary>
        /// Total luminance bits per pixel. This member applies only to luminance-only and luminance-alpha surfaces.
        /// </summary>
        [FieldOffset(12)]
        public uint dwLuminanceBitCount;    // how many bits per pixel

        /// <summary>
        /// Total bump-map bits per pixel in a bump-map surface.
        /// </summary>
        [FieldOffset(12)]
        public uint dwBumpBitCount;         // how many bits per "buxel", total

        /// <summary>
        /// Bits per pixel of private driver formats. Only valid in texture format list and if DDPF_D3DFORMAT is set.
        /// </summary>
        [FieldOffset(12)]
        public uint dwPrivateFormatBitCount;// Bits per pixel of private driver formats. Only valid in texture
        
        #endregion

        #region union

        /// <summary>
        /// Mask for red bits.
        /// </summary>
        [FieldOffset(16)]
        public uint dwRBitMask;             // mask for red bit

        /// <summary>
        /// Mask for Y bits.
        /// </summary>
        [FieldOffset(16)]
        public uint dwYBitMask;             // mask for Y bits

        /// <summary>
        /// Bit depth of the stencil buffer. This member specifies how many bits are reserved within each pixel of the z-buffer for stencil information (the total number of z-bits is equal to dwZBufferBitDepth minus dwStencilBitDepth).
        /// </summary>
        [FieldOffset(16)]
        public uint dwStencilBitDepth;      // how many stencil bits (note: dwZBufferBitDepth-dwStencilBitDepth is total Z-only bits)

        /// <summary>
        /// Mask for luminance bits.
        /// </summary>
        [FieldOffset(16)]
        public uint dwLuminanceBitMask;     // mask for luminance bits

        /// <summary>
        /// Mask for bump-map U-delta bits.
        /// </summary>
        [FieldOffset(16)]
        public uint dwBumpDuBitMask;        // mask for bump map U delta bits

        /// <summary>
        /// Flags that specify the operations that can be performed on surfaces with the DDPF_D3DFORMAT pixel format. The flags are defined in Ddrawi.h.
        /// </summary>
        [FieldOffset(16)]
        public uint dwOperations;           // DDPF_D3DFORMAT Operations
        
        #endregion

        #region union
        
        /// <summary>
        /// Mask for green bits.
        /// </summary>
        [FieldOffset(20)]
        public uint dwGBitMask;             // mask for green bits

        /// <summary>
        /// Mask for U bits.
        /// </summary>
        [FieldOffset(20)]
        public uint dwUBitMask;             // mask for U bits

        /// <summary>
        /// Mask for z bits.
        /// </summary>
        [FieldOffset(20)]
        public uint dwZBitMask;             // mask for Z bits

        /// <summary>
        /// Mask for bump-map V-delta bits.
        /// </summary>
        [FieldOffset(20)]
        public uint dwBumpDvBitMask;        // mask for bump map V delta bits

        /// <summary>
        /// A structure that contains the following two members. This structure is used to specify surfaces that can be used when performing multisample rendering. Each bit in the 16-bit masks indicates support of multisampling with a specific number of samples. For example, bit 0 indicates support of multisampling with only a single sample, bit 1 indicates the support of multisampling with two samples, and so on. The driver can indicate more than one supported level by combining the bits by using a bitwise OR.
        /// </summary>
        [FieldOffset(20)]
        public MultiSampleCaps caps;
        
        #endregion

        #region union

        /// <summary>
        /// Mask for blue bits.
        /// </summary>
        [FieldOffset(24)]
        public uint dwBBitMask;             // mask for blue bits

        /// <summary>
        /// Mask for V bits.
        /// </summary>
        [FieldOffset(24)]
        public uint dwVBitMask;             // mask for V bits
        
        /// <summary>
        /// Mask for stencil bits within each z-buffer pixel.
        /// </summary>
        [FieldOffset(24)]
        public uint dwStencilBitMask;       // mask for stencil bits

        /// <summary>
        /// Mask for luminance in a bump-map pixel.
        /// </summary>
        [FieldOffset(24)]
        public uint dwBumpLuminanceBitMask; // mask for luminance in bump map
        
        #endregion

        #region union
        
        /// <summary>
        /// RGB mask for the alpha channel.
        /// </summary>
        [FieldOffset(28)]
        public uint dwRGBAlphaBitMask;      // mask for alpha channel

        /// <summary>
        /// YUV mask for the alpha channel.
        /// </summary>
        [FieldOffset(28)]
        public uint dwYUVAlphaBitMask;      // mask for alpha channel

        /// <summary>
        /// Luminance mask for the alpha channel.
        /// </summary>
        [FieldOffset(28)]
        public uint dwLuminanceAlphaBitMask;// mask for alpha channel

        /// <summary>
        /// RGB mask for the z channel.
        /// </summary>
        [FieldOffset(28)]
        public uint dwRGBZBitMask;          // mask for Z channel

        /// <summary>
        /// YUV mask for the z channel.
        /// </summary>
        [FieldOffset(28)]
        public uint dwYUVZBitMask;          // mask for Z channel
        
        #endregion
    }

    /// <summary>
    /// A structure that contains the following two members. This structure is used to specify surfaces that can be used when performing multisample rendering. Each bit in the 16-bit masks indicates support of multisampling with a specific number of samples. For example, bit 0 indicates support of multisampling with only a single sample, bit 1 indicates the support of multisampling with two samples, and so on. The driver can indicate more than one supported level by combining the bits by using a bitwise OR.
    /// </summary>
    internal struct MultiSampleCaps
    {
        /// <summary>
        /// A 16-bit mask for full-screen (flip) mode multisampling.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
#pragma warning disable 0649
        public ushort wFlipMSTypes;     // Multisample methods supported via flip for this D3DFORMAT

        /// <summary>
        /// A 16-bit mask for windowed (bit-block transfer) mode multisampling.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public ushort wBltMSTypes;      // Multisample methods supported via blt for this D3DFORMAT
#pragma warning restore 0649
    }
}
