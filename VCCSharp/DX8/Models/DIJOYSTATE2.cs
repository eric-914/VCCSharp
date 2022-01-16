using System.Runtime.InteropServices;

namespace VCCSharp.DX8.Models
{
    [StructLayout(LayoutKind.Explicit, Size = 264, CharSet = CharSet.Ansi)]
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once IdentifierTypo
    public struct DIJOYSTATE2
    {
        /// <summary>
        /// x-axis position
        /// </summary>
        [FieldOffset(0)]
        public int lX;

        /// <summary>
        /// y-axis position
        /// </summary>
        [FieldOffset(4)]
        public int lY;

        /// <summary>
        /// z-axis position
        /// </summary>
        [FieldOffset(8)]
        public int lZ;

        /// <summary>
        /// x-axis rotation 
        /// </summary>
        [FieldOffset(12)]
        public int lRx;

        /// <summary>
        /// y-axis rotation 
        /// </summary>
        [FieldOffset(16)]
        public int lRy;

        /// <summary>
        /// z-axis rotation 
        /// </summary>
        [FieldOffset(20)]
        public int lRz;

        /// <summary>
        /// extra axes positions 
        /// </summary>
        [FieldOffset(24)]
        public unsafe fixed int rglSlider[2];

        /// <summary>
        /// POV directions 
        /// </summary>
        [FieldOffset(32)]
        public unsafe fixed uint rgdwPOV[4];

        /// <summary>
        /// 128 buttons 
        /// </summary>
        [FieldOffset(40)]
        public unsafe fixed byte rgbButtons[128];

        /// <summary>
        /// x-axis velocity 
        /// </summary>
        [FieldOffset(168)]
        public int lVX;

        /// <summary>
        /// y-axis velocity 
        /// </summary>
        [FieldOffset(172)]
        public int lVY;

        /// <summary>
        /// z-axis velocity 
        /// </summary>
        [FieldOffset(176)]
        public int lVZ;

        /// <summary>
        /// x-axis angular velocity 
        /// </summary>
        [FieldOffset(180)]
        public int lVRx;

        /// <summary>
        /// y-axis angular velocity 
        /// </summary>
        [FieldOffset(184)]
        public int lVRy;

        /// <summary>
        /// z-axis angular velocity 
        /// </summary>
        [FieldOffset(188)]
        public int lVRz;

        /// <summary>
        /// extra axes velocities 
        /// </summary>
        [FieldOffset(192)]
        public unsafe fixed int rglVSlider[2];

        /// <summary>
        /// x-axis acceleration 
        /// </summary>
        [FieldOffset(200)]
        public int lAX;

        /// <summary>
        /// y-axis acceleration 
        /// </summary>
        [FieldOffset(204)]
        public int lAY;

        /// <summary>
        /// z-axis acceleration 
        /// </summary>
        [FieldOffset(208)]
        public int lAZ;

        /// <summary>
        /// x-axis angular acceleration 
        /// </summary>
        [FieldOffset(212)]
        public int lARx;

        /// <summary>
        /// y-axis angular acceleration 
        /// </summary>
        [FieldOffset(216)]
        public int lARy;

        /// <summary>
        /// z-axis angular acceleration 
        /// </summary>
        [FieldOffset(220)]
        public int lARz;

        /// <summary>
        /// extra axes accelerations 
        /// </summary>
        [FieldOffset(224)]
        public unsafe fixed int rglASlider[2];

        /// <summary>
        /// x-axis force 
        /// </summary>
        [FieldOffset(232)]
        public int lFX;

        /// <summary>
        /// y-axis force 
        /// </summary>
        [FieldOffset(236)]
        public int lFY;

        /// <summary>
        /// z-axis force 
        /// </summary>
        [FieldOffset(240)]
        public int lFZ;

        /// <summary>
        /// x-axis torque 
        /// </summary>
        [FieldOffset(244)]
        public int lFRx;

        /// <summary>
        /// y-axis torque 
        /// </summary>
        [FieldOffset(248)]
        public int lFRy;

        /// <summary>
        /// z-axis torque 
        /// </summary>
        [FieldOffset(252)]
        public int lFRz;

        /// <summary>
        /// extra axes forces 
        /// </summary>
        [FieldOffset(256)]
        public unsafe fixed int rglFSlider[2];
    }
}
