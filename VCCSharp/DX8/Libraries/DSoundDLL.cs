using System;
using System.Runtime.InteropServices;
using VCCSharp.DX8.Models;

namespace VCCSharp.DX8.Libraries
{
    // ReSharper disable once InconsistentNaming
    public static class DSoundDLL
    {
        public const string Dll = "dsound.dll";

        [DllImport(Dll)]
        public static extern unsafe long DirectSoundCreate(_GUID* pcGuidDevice, IntPtr* ppDS, IntPtr pUnkOuter);

        [DllImport(Dll)]
        public static extern long DirectSoundEnumerate(IntPtr lpDSEnumCallback, IntPtr lpContext);
    }
}
