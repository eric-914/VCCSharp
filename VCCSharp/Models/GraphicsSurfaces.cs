using System.Runtime.InteropServices;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Explicit)]
    public class GraphicsSurfaces
    {
        [FieldOffset(0)]
        public unsafe void* pSurface;
        [FieldOffset(0)]
        public unsafe byte* pSurface8;
        [FieldOffset(0)]
        public unsafe ushort* pSurface16;
        [FieldOffset(0)]
        public unsafe uint* pSurface32;
    }
}
