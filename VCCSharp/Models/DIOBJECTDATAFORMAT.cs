using System.Runtime.InteropServices;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Explicit, Size = 28, CharSet = CharSet.Ansi)]
    public struct DIOBJECTDATAFORMAT
    {
        [FieldOffset(0)]
        public _GUID pguid;

        [FieldOffset(16)]
        public uint dwOfs;

        [FieldOffset(20)]
        public uint dwType;

        [FieldOffset(24)]
        public uint dwFlags;
    }
}