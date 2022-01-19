// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
using System.Runtime.InteropServices;
using LPGUID = System.IntPtr;

namespace DX8.Models
{
    [StructLayout(LayoutKind.Explicit, Size = 24, CharSet = CharSet.Ansi)]
    public struct DIOBJECTDATAFORMAT
    {
        [FieldOffset(0)] //--Length=8
        public LPGUID pguid;

        //--What is hiding @ 8,9,10,11?
        [FieldOffset(8)]
        public uint unknown;

        [FieldOffset(12)]
        public uint dwOfs;

        [FieldOffset(16)]
        public uint dwType;

        [FieldOffset(20)]
        public uint dwFlags;
    }
}
