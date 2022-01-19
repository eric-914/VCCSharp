// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
using System.Runtime.InteropServices;

namespace DX8.Models
{
    /// <summary>
    /// Describes a device object instance. This structure is used with the EnumObjects method to provide the DIEnumDeviceObjectsCallback callback function with information about a particular object associated with a device, such as an axis or button. It is also used with the GetObjectInfo method to retrieve information about a device object.
    /// </summary>
    /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/desktop/ee416612(v=vs.85)"/>
    [StructLayout(LayoutKind.Explicit, Size = 316, CharSet = CharSet.Ansi)]
    public struct DIDEVICEOBJECTINSTANCE
    {
        /// <summary>
        /// Size of the structure, in bytes. During enumeration, the application can inspect this value to determine how many members of the structure are valid. When the structure is passed to the GetObjectInfo method, this member must be initialized to sizeof(DIDEVICEOBJECTINSTANCE).
        /// </summary>
        [FieldOffset(0)]
        public uint dwSize;
        
        /// <summary>
        /// Unique identifier that indicates the object type. This member is optional.
        /// </summary>
        [FieldOffset(4)]
        public _GUID guidType;
        
        /// <summary>
        /// Offset in the native data format of the device. The native data format corresponds to the raw device data. The dwOfs member does not correspond to the device constant, such as DIJOFS_BUTTON0, for this object.
        /// </summary>
        [FieldOffset(20)]
        public uint dwOfs;
        
        /// <summary>
        /// Device type that describes the object. It is a combination of DIDFT_* flags that describe the object type (axis, button, and so on) and contains the object instance number in the middle 16 bits.
        /// </summary>
        [FieldOffset(24)]
        public uint dwType;
        
        /// <summary>
        /// Flags describing other attributes of the data format.
        /// </summary>
        [FieldOffset(28)]
        public uint dwFlags;
        
        /// <summary>
        /// Name of the object; for example, "X-Axis" or "Right Shift."
        /// </summary>
        [FieldOffset(32)]
        public unsafe fixed byte tszName[260];
        
        /// <summary>
        /// The magnitude of the maximum force that can be created by the actuator associated with this object. Force is expressed in newtons and measured in relation to where the hand would be during normal operation of the device.
        /// </summary>
        [FieldOffset(292)]
        public uint dwFFMaxForce;
        
        /// <summary>
        /// The force resolution of the actuator associated with this object. The returned value represents the number of gradations, or subdivisions, of the maximum force that can be expressed by the force-feedback system from 0 (no force) to maximum force.
        /// </summary>
        [FieldOffset(296)]
        public uint dwFFForceResolution;
        
        /// <summary>
        /// The Human Interface Device (human interface device) link collection to which the object belongs.
        /// </summary>
        [FieldOffset(300)]
        public ushort wCollectionNumber;
        
        /// <summary>
        /// An index that refers to a designator in the HID physical descriptor. This number can be passed to functions in the HID parsing library (Hidpi.h) to obtain additional information about the device object.
        /// </summary>
        [FieldOffset(302)]
        public ushort wDesignatorIndex;
        
        /// <summary>
        /// The Human Interface Device (HID) usage page associated with the object, if known. HIDs always report a usage page. Non-HID devices can optionally report a usage page; if they do not, the value of this member is 0.
        /// </summary>
        [FieldOffset(304)]
        public ushort wUsagePage;
        
        /// <summary>
        /// The HID usage associated with the object, if known. HIDs always report a usage. Non-HIDs can optionally report a usage; if they do not, the value of this member is 0.
        /// </summary>
        [FieldOffset(306)]
        public ushort wUsage;
        
        /// <summary>
        /// A Human Interface Device (HID) code for the dimensional units in which the object's value is reported, if known, or 0 if not known.
        /// </summary>
        [FieldOffset(308)]
        public uint dwDimension;
        
        /// <summary>
        /// The exponent to associate with the dimension, if known. Dimensional units are always integral, so an exponent might be needed to convert them to nonintegral types.
        /// </summary>
        [FieldOffset(312)]
        public ushort wExponent;

        /// <summary>
        /// Reserved.
        /// </summary>
        [FieldOffset(314)]
        public ushort wReportId;
    }
}
