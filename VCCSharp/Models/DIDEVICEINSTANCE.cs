using System.Runtime.InteropServices;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Explicit, Size = 580, CharSet = CharSet.Ansi)]
    public struct DIDEVICEINSTANCE
    {
        [FieldOffset(0)]
        public uint dwSize;

        [FieldOffset(4)]
        public _GUID guidInstance;
        
        [FieldOffset(20)]
        public _GUID guidProduct;
        
        [FieldOffset(36)]
        public uint dwDevType;

        [FieldOffset(40)]
        public unsafe fixed byte tszInstanceName[260];

        [FieldOffset(300)]
        public unsafe fixed byte tszProductName[260];

        [FieldOffset(560)]
        public _GUID guidFFDriver;

        [FieldOffset(576)]
        public ushort wUsagePage;

        [FieldOffset(578)]
        public ushort wUsage;
    }
}
