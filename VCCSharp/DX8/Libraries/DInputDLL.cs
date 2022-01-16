using System;
using System.Runtime.InteropServices;
using VCCSharp.DX8.Models;

namespace VCCSharp.DX8.Libraries
{
    // ReSharper disable once InconsistentNaming
    public static class DInputDLL
    {
        public const string Dll = "dinput8.dll";

        [DllImport(Dll)]
        public static extern int DirectInput8Create(IntPtr handle, uint version, _GUID pGuid, ref IntPtr pInstance, IntPtr pUnknown);
    }
}
