// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo

using System.Runtime.InteropServices;

namespace DX8.Internal.Models
{
    /// <summary>
    /// Contains information about the range of an object within a device.
    /// </summary>
    /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/desktop/ee416640(v=vs.85)"/>
    [StructLayout(LayoutKind.Explicit, Size = MEMBLOCK.DIPROPRANGE, CharSet = CharSet.Ansi)]
    internal struct DIPROPRANGE
    {
        /// <summary>
        /// DIPROPHEADER structure.
        /// </summary>
        [FieldOffset(0)]
        public DIPROPHEADER diph;

        /// <summary>
        /// Lower limit of the range. If the range of the device is unrestricted, this value is DIPROPRANGE_NOMIN when the GetProperty method returns.
        /// </summary>
        [FieldOffset(16)]
        public int lMin;

        /// <summary>
        /// Upper limit of the range. If the range of the device is unrestricted, this value is DIPROPRANGE_NOMAX when the GetProperty method returns.
        /// </summary>
        [FieldOffset(20)]
        public int lMax;

        public static int Size => MEMBLOCK.DIPROPRANGE;
    }
}
