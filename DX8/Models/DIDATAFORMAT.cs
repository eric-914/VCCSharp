// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
using System.Runtime.InteropServices;

namespace DX8.Models
{
    /// <summary>
    /// Describes a device's data format. This structure is used with the SetDataFormat method.
    /// </summary>
    /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/desktop/ee416606(v=vs.85)"/>
    [StructLayout(LayoutKind.Explicit, Size = 32, CharSet = CharSet.Ansi)]
    public struct DIDATAFORMAT
    {
        /// <summary>
        /// Size of this structure, in bytes.
        /// </summary>
        [FieldOffset(0)]
        public uint dwSize;

        /// <summary>
        /// Size of the DIOBJECTDATAFORMAT structure, in bytes.
        /// </summary>
        [FieldOffset(4)]
        public uint dwObjSize;

        /// <summary>
        /// Flags describing other attributes of the data format.
        /// </summary>
        [FieldOffset(8)]
        public uint dwFlags;

        /// <summary>
        /// Size of a data packet returned by the device, in bytes. This value must be a multiple of 4 and must exceed the largest offset value for an object's data within the data packet.
        /// </summary>
        [FieldOffset(12)]
        public uint dwDataSize;

        /// <summary>
        /// Number of objects in the rgodf array.
        /// </summary>
        [FieldOffset(16)]
        public uint dwNumObjs;

        //--Mystery gap @ 20,21,22,23

        /// <summary>
        /// Address to an array of DIOBJECTDATAFORMAT structures. Each structure describes how one object's data should be reported in the device data. Typical errors include placing two pieces of information in the same location and placing one piece of information in more than one location.
        /// </summary>
        [FieldOffset(24)]
        public unsafe DIOBJECTDATAFORMAT* rgodf;
    }
}
