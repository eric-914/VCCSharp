using System;
using System.Runtime.InteropServices;

namespace DX8.Models
{
    [StructLayout(LayoutKind.Explicit, Size = 40, CharSet = CharSet.Ansi)]
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once IdentifierTypo
    public struct DSBUFFERDESC
    {
        [FieldOffset(0)]
        public uint dwSize;

        [FieldOffset(4)]
        public uint dwFlags;

        [FieldOffset(8)]
        public uint dwBufferBytes;

        [FieldOffset(12)]
        public uint dwReserved;

        [FieldOffset(16)]
        public IntPtr lpwfxFormat; //--WAVEFORMATEX*

        public static unsafe int Size => sizeof(DSBUFFERDESC);
    }
}
