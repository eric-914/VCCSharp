using System;
using System.Runtime.InteropServices;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Explicit, Size = 40, CharSet = CharSet.Ansi)]
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
    }
}