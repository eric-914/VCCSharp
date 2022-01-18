using System;
using System.Runtime.InteropServices;
using DX8.Models;

namespace DX8.Libraries
{
    // ReSharper disable once InconsistentNaming
    public static class DSoundDLL
    {
        public const string Dll = "dsound.dll";

        [DllImport(Dll)]
        public static extern unsafe long DirectSoundCreate(_GUID* pGuid, ref IntPtr pInstance, IntPtr pUnknown);

        [DllImport(Dll)]
        public static extern long DirectSoundEnumerate(IntPtr pCallback, IntPtr pContext);
    }
}
