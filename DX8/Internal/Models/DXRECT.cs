// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace DX8.Internal.Models
{
    /// <summary>
    /// The (DX)RECT structure defines a rectangle by the coordinates of its upper-left and lower-right corners.
    /// Same as RECT, but kept separate as to not create a dependency
    /// </summary>
    /// <see href="https://docs.microsoft.com/en-us/windows/win32/api/windef/ns-windef-rect"/>
    internal struct DXRECT
    {
        /// <summary>
        /// Specifies the x-coordinate of the upper-left corner of the rectangle.
        /// </summary>
        public int left;

        /// <summary>
        /// Specifies the y-coordinate of the upper-left corner of the rectangle.
        /// </summary>
        public int top;

        /// <summary>
        /// Specifies the x-coordinate of the lower-right corner of the rectangle.
        /// </summary>
        public int right;

        /// <summary>
        /// Specifies the y-coordinate of the lower-right corner of the rectangle.
        /// </summary>
        public int bottom;
    }
}
