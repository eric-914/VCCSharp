// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo

namespace DX8.Internal.Models
{
    /// <summary>
    /// The DDSCAPS structure defines the capabilities of a Microsoft DirectDraw surface object.
    /// </summary>
    /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/hardware/drivers/ff550286(v=vs.85)"/>
    internal struct DDSCAPS
    {
        /// <summary>
        /// Indicates a set of flags that specify the capabilities of the surface.
        /// </summary>
        public uint dwCaps;         // capabilities of surface wanted
    }
}
