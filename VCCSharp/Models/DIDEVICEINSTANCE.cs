using System.Runtime.InteropServices;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DIDEVICEINSTANCE
    {
        public ulong dwSize;
        public _GUID guidInstance;
        public _GUID guidProduct;
        public ulong dwDevType;
        public unsafe fixed byte tszInstanceName[260];
        public unsafe fixed byte tszProductName[260];

        public _GUID guidFFDriver;
        public ushort wUsagePage;
        public ushort wUsage;
    }
}
