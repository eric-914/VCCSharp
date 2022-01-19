// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace DX8.Models
{
    /// <summary>
    /// Same as RECT, but kept separate as to not create a dependency
    /// </summary>
    public struct DXRECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
}
