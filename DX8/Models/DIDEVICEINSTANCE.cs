// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
using System.Runtime.InteropServices;

namespace DX8.Models
{
    /// <summary>
    /// Describes an instance of a DirectInput device. This structure is used with the EnumDevices, EnumDevicesBySemantics, and GetDeviceInfo methods.
    /// </summary>
    /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/desktop/ee416610(v=vs.85)"/>
    [StructLayout(LayoutKind.Explicit, Size = 580, CharSet = CharSet.Ansi)]
    public struct DIDEVICEINSTANCE
    {
        /// <summary>
        /// Size of this structure, in bytes. This member must be initialized before the structure is used.
        /// </summary>
        [FieldOffset(0)]
        public uint dwSize;

        /// <summary>
        /// Unique identifier for the instance of the device. An application can save the instance globally unique identifier (GUID) into a configuration file and use it at a later time. Instance GUIDs are specific to a particular computer. An instance GUID obtained from one computer is unrelated to instance GUIDs on another.
        /// </summary>
        [FieldOffset(4)]
        public _GUID guidInstance;

        /// <summary>
        /// Unique identifier for the product. This identifier is established by the manufacturer of the device.
        /// </summary>
        [FieldOffset(20)]
        public _GUID guidProduct;

        /// <summary>
        /// Device type specifier. The least-significant byte of the device type description code specifies the device type. The next-significant byte specifies the device subtype. This value can also be combined with DIDEVTYPE_HID, which specifies a Human Interface Device (human interface device).
        /// </summary>
        [FieldOffset(36)]
        public uint dwDevType;

        /// <summary>
        /// Friendly name for the instance. For example, "Joystick 1."
        /// </summary>
        [FieldOffset(40)]
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 260)]
        public byte[] tszInstanceName;

        //TODO: Any attempt to make this an array fails.  But I'm not using it, so is it worth pursuing?
        ///// <summary>
        ///// Friendly name for the product.
        ///// </summary>
        //[FieldOffset(300)]
        //[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeConst = 260)]
        //public byte[] tszProductName;

        /// <summary>
        /// Unique identifier for the driver being used for force feedback. The driver's manufacturer establishes this identifier.
        /// </summary>
        [FieldOffset(560)]
        public _GUID guidFFDriver;

        /// <summary>
        /// If the device is a Human Interface Device (HID), this member contains the HID usage page code.
        /// </summary>
        [FieldOffset(576)]
        public ushort wUsagePage;

        /// <summary>
        /// If the device is a Human Interface Device (HID), this member contains the HID usage code.
        /// </summary>
        [FieldOffset(578)]
        public ushort wUsage;
    }
}
