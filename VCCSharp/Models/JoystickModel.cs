using System.Runtime.InteropServices;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct JoystickModel
    {
        // 0 -- Keyboard
        // 1 -- Mouse
        // 2 -- Audio 
        // 3 -- Joystick 
        public byte UseMouse;

        // Index of which Joystick is selected
        public byte DiDevice;

        // 0 -- Standard,
        // 1 -- TandyHiRes,
        // 2 -- CCMAX
        public byte HiRes;  //TODO: This doesn't seem bound to anything

        public byte Up;
        public byte Down;
        public byte Left;
        public byte Right;
        public byte Fire1;
        public byte Fire2;
    }
}
