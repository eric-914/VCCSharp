using System.Runtime.InteropServices;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Explicit, Size = 58, CharSet = CharSet.Ansi)]
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once IdentifierTypo
    public struct DIDATAFORMAT
    {
        [FieldOffset(0)]
        public uint dwSize;

        [FieldOffset(4)]
        public uint dwObjSize;

        [FieldOffset(8)]
        public uint dwFlags;

        [FieldOffset(12)]
        public uint dwDataSize;

        [FieldOffset(16)]
        public uint dwNumObjs;

        [FieldOffset(20)]
        public unsafe DIOBJECTDATAFORMAT rgodf;
    }
}
