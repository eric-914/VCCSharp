using System;
using System.Runtime.InteropServices;
using VCCSharp.Models;

namespace VCCSharp.Libraries
{
    public static class DSoundDLL
    {
        public const string Dll = "DSound.dll";

        [DllImport(Dll)]
        public static extern unsafe long DirectSoundCreate(_GUID* pcGuidDevice, IntPtr* ppDS, IntPtr pUnkOuter);
    }
}
