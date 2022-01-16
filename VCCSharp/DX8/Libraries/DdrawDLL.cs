using System;
using System.Runtime.InteropServices;

namespace VCCSharp.DX8.Libraries
{
    // ReSharper disable once InconsistentNaming
    public static class DDrawDLL
    {
        public const string Dll = "ddraw.dll";

        [DllImport(Dll)]
        public static extern unsafe int DirectDrawCreate(IntPtr lpGUID, IntPtr* lplpDD, IntPtr pUnkOuter);
    }
}
