using System.Runtime.InteropServices;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    // ReSharper disable once InconsistentNaming
    public struct _GUID
    {
        public uint Data1;
        public ushort Data2;
        public ushort Data3;
        public unsafe fixed byte Data4[8];
    }
}
