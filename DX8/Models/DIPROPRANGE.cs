using System.Runtime.InteropServices;

namespace DX8.Models
{
    [StructLayout(LayoutKind.Explicit, Size = 24, CharSet = CharSet.Ansi)]
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once IdentifierTypo
    public struct DIPROPRANGE
    {
        [FieldOffset(0)]
        public DIPROPHEADER diph;

        [FieldOffset(16)]
        public int lMin;

        [FieldOffset(20)]
        public int lMax;

        public static unsafe int Size => sizeof(DIPROPRANGE);
    }
}
