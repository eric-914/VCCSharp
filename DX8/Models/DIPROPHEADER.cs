// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
using System.Runtime.InteropServices;

namespace DX8.Models
{
    /// <summary>
    /// Serves as a header for all property structures.
    /// </summary>
    /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/desktop/ee416638(v=vs.85)"/>
    [StructLayout(LayoutKind.Explicit, Size = 16, CharSet = CharSet.Ansi)]
    public struct DIPROPHEADER
    {
        /// <summary>
        /// Size of the enclosing structure. This member must be initialized before the structure is used.
        /// </summary>
        [FieldOffset(0)]
        public uint dwSize;

        /// <summary>
        /// Size of the DIPROPHEADER structure.
        /// </summary>
        [FieldOffset(4)]
        public uint dwHeaderSize;

        /// <summary>
        /// Object for which the property is to be accessed. The value set for this member depends on the value specified in the dwHow member.
        /// </summary>
        [FieldOffset(8)]
        public uint dwObj;

        /// <summary>
        /// Value that specifies how the dwObj member should be interpreted.
        /// </summary>
        [FieldOffset(12)]
        public uint dwHow;

        public static unsafe int Size => sizeof(DIPROPHEADER);
    }
}
