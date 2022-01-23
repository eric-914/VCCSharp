// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo

using System.Runtime.InteropServices;
using LPWAVEFORMATEX = System.IntPtr;

namespace DX8.Internal.Models
{
    /// <summary>
    /// The DSBUFFERDESC structure describes the characteristics of a new buffer object.
    /// </summary>
    /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/desktop/ee416820(v=vs.85)"/>
    [StructLayout(LayoutKind.Explicit, Size = MEMBLOCK.DSBUFFERDESC, CharSet = CharSet.Ansi)]
    internal struct DSBUFFERDESC
    {
        /// <summary>
        /// Size of the structure, in bytes. This member must be initialized before the structure is used.
        /// </summary>
        [FieldOffset(0)]
        public uint dwSize;

        /// <summary>
        /// Flags specifying the capabilities of the buffer. See the dwFlags member of the DSBCAPS structure for a detailed listing of valid flags.
        /// </summary>
        [FieldOffset(4)]
        public uint dwFlags;

        /// <summary>
        /// Size of the new buffer, in bytes. This value must be 0 when creating a buffer with the DSBCAPS_PRIMARYBUFFER flag. For secondary buffers, the minimum and maximum sizes allowed are specified by DSBSIZE_MIN and DSBSIZE_MAX, defined in Dsound.h.
        /// </summary>
        [FieldOffset(8)]
        public uint dwBufferBytes;

        /// <summary>
        /// Reserved. Must be 0.
        /// </summary>
        [FieldOffset(12)]
        public uint dwReserved;

        /// <summary>
        /// Address of a WAVEFORMATEX or WAVEFORMATEXTENSIBLE structure specifying the waveform format for the buffer. This value must be NULL for primary buffers.
        /// </summary>
        [FieldOffset(16)]
        public LPWAVEFORMATEX lpwfxFormat; //--WAVEFORMATEX*

        /// <summary>
        /// Unique identifier of the two-speaker virtualization algorithm to be used by DirectSound3D hardware emulation. If DSBCAPS_CTRL3D is not set in dwFlags, this member must be GUID_NULL (DS3DALG_DEFAULT).
        /// </summary>
        [FieldOffset(24)]
        public _GUID guid3DAlgorithm;

        public static int Size => MEMBLOCK.DSBUFFERDESC;
    }
}
