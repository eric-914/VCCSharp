using System;
using System.Runtime.InteropServices;

namespace DX8.Libraries
{
    // ReSharper disable once InconsistentNaming
    public static class DDrawDLL
    {
        public const string Dll = "ddraw.dll";

        [DllImport(Dll)]
        public static extern int DirectDrawCreate(IntPtr pGuid, ref IntPtr pInstance, IntPtr pUnknown);
    }
}
