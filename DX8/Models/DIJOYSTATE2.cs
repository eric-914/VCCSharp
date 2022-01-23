// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
using System.Runtime.InteropServices;

namespace DX8.Models
{
    /// <summary>
    /// Describes the state of a joystick device with extended capabilities. This structure is used with the GetDeviceState method.
    /// </summary>
    /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/desktop/ee416628(v=vs.85)"/>
    [StructLayout(LayoutKind.Explicit, Size = MEMBLOCK.DIJOYSTATE2, CharSet = CharSet.Ansi)]
    public struct DIJOYSTATE2
    {
        /// <summary>
        /// X-axis, usually the left-right movement of a stick.
        /// </summary>
        [FieldOffset(0)]
        public int lX;  // x-axis position

        /// <summary>
        /// Y-axis, usually the forward-backward movement of a stick.
        /// </summary>
        [FieldOffset(4)]
        public int lY;  // y-axis position

        /// <summary>
        /// Z-axis, often the throttle control. If the joystick does not have this axis, the value is 0.
        /// </summary>
        [FieldOffset(8)]
        public int lZ;  // z-axis position

        /// <summary>
        /// X-axis rotation. If the joystick does not have this axis, the value is 0.
        /// </summary>
        [FieldOffset(12)]
        public int lRx; // x-axis rotation 

        /// <summary>
        /// Y-axis rotation. If the joystick does not have this axis, the value is 0.
        /// </summary>
        [FieldOffset(16)]
        public int lRy; // y-axis rotation 

        /// <summary>
        /// Z-axis rotation (often called the rudder). If the joystick does not have this axis, the value is 0.
        /// </summary>
        [FieldOffset(20)]
        public int lRz; // z-axis rotation 

        /// <summary>
        /// Two additional axis values (formerly called the u-axis and v-axis) whose semantics depend on the joystick. Use the GetObjectInfo method to obtain semantic information about these values.
        /// </summary>
        [FieldOffset(24)]
        public INT2 rglSlider;   // extra axes positions 

        /// <summary>
        /// Direction controllers, such as point-of-view hats. The position is indicated in hundredths of a degree clockwise from north (away from the user). The center position is normally reported as - 1; but see Remarks. For indicators that have only five positions, the value for a controller is - 1, 0, 9,000, 18,000, or 27,000.
        /// </summary>
        [FieldOffset(32)]
        public UINT4 rgdwPOV;    // POV directions 

        /// <summary>
        /// Array of buttons. The high-order bit of the byte is set if the corresponding button is down, and clear if the button is up or does not exist.
        /// </summary>
        [FieldOffset(40)]
        //[MarshalAs(UnmanagedType.I1, SizeConst = 128)]
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 128)]
        public byte[] rgbButtons;   // 128 buttons 

        /// <summary>
        /// X-axis velocity.
        /// </summary>
        [FieldOffset(168)]
        public int lVX;

        /// <summary>
        /// Y-axis velocity.
        /// </summary>
        [FieldOffset(172)]
        public int lVY;

        /// <summary>
        /// Z-axis velocity.
        /// </summary>
        [FieldOffset(176)]
        public int lVZ;

        /// <summary>
        /// X-axis angular velocity.
        /// </summary>
        [FieldOffset(180)]
        public int lVRx;

        /// <summary>
        /// Y-axis angular velocity.
        /// </summary>
        [FieldOffset(184)]
        public int lVRy;

        /// <summary>
        /// Z-axis angular velocity.
        /// </summary>
        [FieldOffset(188)]
        public int lVRz;

        /// <summary>
        /// Extra axis velocities.
        /// </summary>
        [FieldOffset(192)]
        public INT2 rglVSlider;

        /// <summary>
        /// X-axis acceleration.
        /// </summary>
        [FieldOffset(200)]
        public int lAX;

        /// <summary>
        /// Y-axis acceleration.
        /// </summary>
        [FieldOffset(204)]
        public int lAY;

        /// <summary>
        /// Z-axis acceleration.
        /// </summary>
        [FieldOffset(208)]
        public int lAZ;

        /// <summary>
        /// X-axis angular acceleration.
        /// </summary>
        [FieldOffset(212)]
        public int lARx;

        /// <summary>
        /// Y-axis angular acceleration.
        /// </summary>
        [FieldOffset(216)]
        public int lARy;

        /// <summary>
        /// Z-axis angular acceleration.
        /// </summary>
        [FieldOffset(220)]
        public int lARz;

        /// <summary>
        /// Extra axis accelerations.
        /// </summary>
        [FieldOffset(224)]
        public INT2 rglASlider;

        /// <summary>
        /// X-axis force.
        /// </summary>
        [FieldOffset(232)]
        public int lFX;

        /// <summary>
        /// Y-axis force.
        /// </summary>
        [FieldOffset(236)]
        public int lFY;

        /// <summary>
        /// Z-axis force.
        /// </summary>
        [FieldOffset(240)]
        public int lFZ;

        /// <summary>
        /// X-axis torque.
        /// </summary>
        [FieldOffset(244)]
        public int lFRx;

        /// <summary>
        /// Y-axis torque.
        /// </summary>
        [FieldOffset(248)]
        public int lFRy;

        /// <summary>
        /// Z-axis torque.
        /// </summary>
        [FieldOffset(252)]
        public int lFRz;

        /// <summary>
        /// Extra axis forces.
        /// </summary>
        [FieldOffset(256)]
        public INT2 rglFSlider;

        public static int Size => MEMBLOCK.DIJOYSTATE2;
    }
}
