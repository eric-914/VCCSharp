#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming
// ReSharper disable CommentTypo
using DX8.Converters;
using System.Runtime.InteropServices;

namespace DX8.Models
{
    /// <summary>
    /// Describes a globally unique identifier (GUID).
    /// </summary>
    /// <see href="https://docs.microsoft.com/en-us/office/client-developer/outlook/mapi/guid"/>
    /// <remarks>The GUID structure is defined in the Win32 Programmer's Reference . Specific values for GUID structures that are used within MAPI are defined in the MAPI header file Mapiguid.h.</remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct _GUID
    {
        /// <summary>
        /// An unsigned long integer data value.
        /// </summary>
        public uint Data1;

        /// <summary>
        /// An unsigned short integer data value.
        /// </summary>
        public ushort Data2;

        /// <summary>
        /// An unsigned short integer data value.
        /// </summary>
        public ushort Data3;

        /// <summary>
        /// An array of unsigned characters.
        /// </summary>
        public BYTE8 Data4;

        /// <summary>
        /// For debugging purposes
        /// </summary>
        public string Guid => GuidConverter.ToString(this);
    }
}
