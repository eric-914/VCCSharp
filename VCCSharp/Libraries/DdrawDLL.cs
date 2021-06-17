using System;
using System.Runtime.InteropServices;

namespace VCCSharp.Libraries
{
    public static class DdrawDLL
    {
        public const string Dll = "Ddraw.dll";

        [DllImport(Dll)]
        public static extern unsafe int DirectDrawCreate(IntPtr lpGUID, IntPtr* lplpDD, IntPtr pUnkOuter);
    }
}
