using System.Runtime.InteropServices;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    // ReSharper disable once InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles
    public struct _GUID
    {
        public uint Data1;
        public ushort Data2;
        public ushort Data3;
        public unsafe fixed byte Data4[8];

        private string D(int i)
        {
            unsafe
            {
                var s = $"{Data4[i]:X}";
                return s.Substring(s.Length - 2, 2);
            }
        }

        public override string ToString()
        {
            return $"{Data1:X8}-{Data2:X}-{Data3:X}-{D(0)}{D(1)}-{D(2)}{D(3)}{D(4)}{D(5)}{D(6)}{D(7)}";
        }
    }
}
