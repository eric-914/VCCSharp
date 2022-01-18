using System.Runtime.InteropServices;

namespace DX8.Models
{
    [StructLayout(LayoutKind.Explicit, Size = 16, CharSet = CharSet.Ansi)]
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once IdentifierTypo
    public struct DIPROPHEADER
    {
        [FieldOffset(0)]
        public uint dwSize;

        [FieldOffset(4)]
        public uint dwHeaderSize;

        [FieldOffset(8)]
        public uint dwObj;

        [FieldOffset(12)]
        public uint dwHow;
    }
}
