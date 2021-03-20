using System.Runtime.InteropServices;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct JoystickModel
    {
        public byte UseMouse;
        public byte DiDevice;
        public byte HiRes;

        public byte Up;
        public byte Down;
        public byte Left;
        public byte Right;
        public byte Fire1;
        public byte Fire2;
    }
}
