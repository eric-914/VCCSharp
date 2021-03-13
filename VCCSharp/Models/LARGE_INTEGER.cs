using System;
using System.Runtime.InteropServices;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Explicit, Size=8)]
    public struct LARGE_INTEGER
    {
        [FieldOffset(0)]public Int64 QuadPart;
        [FieldOffset(0)]public UInt32 LowPart;
        [FieldOffset(4)]public Int32 HighPart;
    }
}
