// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
using System.Runtime.InteropServices;
using LPGUID = System.IntPtr;

namespace DX8.Models
{
    /// <summary>
    /// Describes a device object's data format for use with the SetDataFormat method.
    /// </summary>
    /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/desktop/ee416633(v=vs.85)"/>
    [StructLayout(LayoutKind.Explicit, Size = 24, CharSet = CharSet.Ansi)]
    public struct DIOBJECTDATAFORMAT
    {
        /// <summary>
        /// Unique identifier for the axis, button, or other input source. When requesting a data format, making this member NULL indicates that any type of object is permissible.
        /// </summary>
        [FieldOffset(0)] //--Length=8
        public LPGUID pguid;

        //--What is hiding @ 8,9,10,11?
        [FieldOffset(8)]
        public uint unknown;

        /// <summary>
        /// Offset within the data packet where the data for the input source is stored. This value must be a multiple of 4 for DWORD size data, such as axes. It can be byte-aligned for buttons.
        /// </summary>
        [FieldOffset(12)]
        public uint dwOfs;

        /// <summary>
        /// Device type that describes the object. It is a combination of the following flags describing the object type (axis, button, and so forth) and containing the object-instance number in the middle 16 bits. 
        /// </summary>
        [FieldOffset(16)]
        public uint dwType;

        /// <summary>
        /// Zero or more of the following values:
        /// DIDOI_ASPECTACCEL | DIDOI_ASPECTFORCE | DIDOI_ASPECTPOSITION | DIDOI_ASPECTVELOCITY
        /// </summary>
        [FieldOffset(20)]
        public uint dwFlags;
    }
}
